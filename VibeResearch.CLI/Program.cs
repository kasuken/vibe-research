using System.Text;
using Microsoft.Extensions.Configuration;
using OpenAI.Responses;
using Spectre.Console;

// Display application header
AnsiConsole.Write(
    new FigletText("Vibe Research")
        .Centered()
        .Color(Color.Cyan1));

AnsiConsole.MarkupLine("[grey]Deep Research powered by OpenAI Reasoning[/]");
AnsiConsole.WriteLine();

// Load configuration from appsettings.json (if present)
var config = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
    .AddEnvironmentVariables()
    .Build();

// Get OpenAI API key
var apiKey = config["OpenAI:ApiKey"] ?? Environment.GetEnvironmentVariable("OPENAI_API_KEY");
if (string.IsNullOrEmpty(apiKey))
{
    apiKey = AnsiConsole.Prompt(
        new TextPrompt<string>("Enter your [green]OpenAI API Key[/]:")
            .PromptStyle("yellow")
            .Secret());
}

// Get research topic from user
var researchTopic = AnsiConsole.Prompt(
    new TextPrompt<string>("Enter your [cyan]research topic[/]:")
        .PromptStyle("green")
        .Validate(topic =>
            string.IsNullOrWhiteSpace(topic)
                ? ValidationResult.Error("[red]Please enter a valid topic[/]")
                : ValidationResult.Success()));

AnsiConsole.WriteLine();

// Configure research depth (maps to reasoning effort)
var researchDepth = AnsiConsole.Prompt(
    new SelectionPrompt<string>()
        .Title("Select [cyan]research depth[/]:")
        .AddChoices("Quick Overview", "Standard Research", "Deep Dive"));

var (maxIterations, reasoningEffort) = researchDepth switch
{
    "Quick Overview" => (2, ResponseReasoningEffortLevel.Low),
    "Standard Research" => (4, ResponseReasoningEffortLevel.Medium),
    "Deep Dive" => (6, ResponseReasoningEffortLevel.High),
    _ => (3, ResponseReasoningEffortLevel.Medium)
};

AnsiConsole.WriteLine();

// Initialize OpenAI Responses client with reasoning model
var client = new ResponsesClient(
    model: "gpt-5.1",
    apiKey: apiKey);

var researchResults = new StringBuilder();
researchResults.AppendLine($"# Deep Research: {researchTopic}");
researchResults.AppendLine();
researchResults.AppendLine($"*Generated on: {DateTime.Now:yyyy-MM-dd HH:mm:ss}*");
researchResults.AppendLine();
researchResults.AppendLine($"*Reasoning Effort: {researchDepth}*");
researchResults.AppendLine();
researchResults.AppendLine("---");
researchResults.AppendLine();

var conversationHistory = new List<ResponseItem>();

var systemPrompt = """
    You are an expert research assistant conducting deep research on topics. 
    Your goal is to provide comprehensive, well-structured, and accurate information.
    
    For each research iteration:
    1. Explore different aspects and perspectives of the topic
    2. Provide detailed analysis with facts and insights
    3. Identify related subtopics that need further exploration
    4. Use markdown formatting for better readability
    5. Avoid lists unless necessary to organize information, and prefer paragraphs for explanations
    6. Create tables when needed to organize information clearly, otherwise avoid unnecessary formatting
    7. Cite sources and provide references for the information presented
    
    Be thorough, analytical, and provide actionable insights where applicable.
    """;

await AnsiConsole.Status()
    .Spinner(Spinner.Known.Dots)
    .SpinnerStyle(Style.Parse("cyan"))
    .StartAsync("Initializing research...", async ctx =>
    {
        for (int iteration = 1; iteration <= maxIterations; iteration++)
        {
            ctx.Status($"[cyan]Research iteration {iteration}/{maxIterations}...[/]");

            var prompt = iteration == 1
                ? $"{systemPrompt}\n\nConduct comprehensive research on the following topic: \"{researchTopic}\". Provide an overview, key concepts, and identify important subtopics to explore further."
                : "Continue the research by exploring additional aspects, providing deeper analysis, addressing any gaps, and expanding on the subtopics mentioned. Add new insights and perspectives.";

            var response = new StringBuilder();
            var reasoningStatus = new StringBuilder();
            
            try
            {
                var options = new CreateResponseOptions()
                {
                    ReasoningOptions = new ResponseReasoningOptions()
                    {
                        ReasoningEffortLevel = reasoningEffort,
                    },
                    StreamingEnabled = true,
                };

                // Add conversation history
                foreach (var item in conversationHistory)
                {
                    options.InputItems.Add(item);
                }
                
                options.InputItems.Add(ResponseItem.CreateUserMessageItem(prompt));

                await foreach (StreamingResponseUpdate update in client.CreateResponseStreamingAsync(options))
                {
                    if (update is StreamingResponseOutputItemAddedUpdate itemUpdate
                        && itemUpdate.Item is ReasoningResponseItem reasoningItem)
                    {
                        reasoningStatus.AppendLine($"[Reasoning] ({reasoningItem.Status})");
                    }
                    else if (update is StreamingResponseOutputItemAddedUpdate itemDone
                        && itemDone.Item is ReasoningResponseItem reasoningDone)
                    {
                        reasoningStatus.AppendLine($"[Reasoning DONE] ({reasoningDone.Status})");
                    }
                    else if (update is StreamingResponseOutputTextDeltaUpdate delta)
                    {
                        response.Append(delta.Delta);
                    }
                }

                var responseText = response.ToString();
                
                // Add to conversation history
                conversationHistory.Add(ResponseItem.CreateUserMessageItem(prompt));
                conversationHistory.Add(ResponseItem.CreateAssistantMessageItem(responseText));

                researchResults.AppendLine($"## Research Phase {iteration}");
                researchResults.AppendLine();
                if (reasoningStatus.Length > 0)
                {
                    researchResults.AppendLine("```");
                    researchResults.AppendLine(reasoningStatus.ToString().Trim());
                    researchResults.AppendLine("```");
                    researchResults.AppendLine();
                }
                researchResults.AppendLine(responseText);
                researchResults.AppendLine();
                researchResults.AppendLine("---");
                researchResults.AppendLine();

                // Brief pause between iterations
                if (iteration < maxIterations)
                {
                    await Task.Delay(500);
                }
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Error during research iteration {iteration}: {ex.Message}[/]");
                break;
            }
        }

        // Generate summary
        ctx.Status("[cyan]Generating research summary...[/]");
        
        var summaryPrompt = "Provide a concise executive summary of all the research conducted, highlighting the key findings, main conclusions, and recommended next steps. Add a list with all the sources and references used during the research.";
        var summaryResponse = new StringBuilder();
        
        try
        {
            var summaryOptions = new CreateResponseOptions()
            {
                ReasoningOptions = new ResponseReasoningOptions()
                {
                    ReasoningEffortLevel = reasoningEffort,
                },
                StreamingEnabled = true,
            };

            foreach (var item in conversationHistory)
            {
                summaryOptions.InputItems.Add(item);
            }
            
            summaryOptions.InputItems.Add(ResponseItem.CreateUserMessageItem(summaryPrompt));

            await foreach (StreamingResponseUpdate update in client.CreateResponseStreamingAsync(summaryOptions))
            {
                if (update is StreamingResponseOutputTextDeltaUpdate delta)
                {
                    summaryResponse.Append(delta.Delta);
                }
            }

            researchResults.AppendLine("## Executive Summary");
            researchResults.AppendLine();
            researchResults.AppendLine(summaryResponse.ToString());
            researchResults.AppendLine();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error generating summary: {ex.Message}[/]");
        }
    });

// Save to markdown file
var sanitizedTopic = string.Join("_", researchTopic.Split(Path.GetInvalidFileNameChars()));
var fileName = $"research_{sanitizedTopic}_{DateTime.Now:yyyyMMdd_HHmmss}.md";
var outputPath = Path.Combine(Environment.CurrentDirectory, fileName);

await File.WriteAllTextAsync(outputPath, researchResults.ToString());

// Generate HTML version
var htmlFileName = $"research_{sanitizedTopic}_{DateTime.Now:yyyyMMdd_HHmmss}.html";
var htmlOutputPath = Path.Combine(Environment.CurrentDirectory, htmlFileName);

await AnsiConsole.Status()
    .Spinner(Spinner.Known.Dots)
    .SpinnerStyle(Style.Parse("cyan"))
    .StartAsync("Generating HTML report...", async _ =>
    {
        var htmlPrompt = $"""
            Convert the following markdown research document into a beautiful, professional HTML page.
            
            Requirements:
            - Create a complete, standalone HTML5 document
            - Include embedded CSS styles (no external stylesheets)
            - Use a clean, modern design with good typography
            - Use a professional color scheme (dark header, light background for content)
            - Make it responsive for different screen sizes
            - Include a table of contents with anchor links to each section
            - Style code blocks, tables, and lists appropriately
            - Add a footer with generation date and "Generated by Vibe Research"
            - Use semantic HTML elements (header, main, article, section, footer, nav)
            
            Return ONLY the complete HTML code, no explanations or markdown code blocks.
            
            Markdown content:
            {researchResults}
            """;

        var htmlResponse = new StringBuilder();
        
        try
        {
            var htmlOptions = new CreateResponseOptions()
            {
                StreamingEnabled = true
            };

            htmlOptions.InputItems.Add(ResponseItem.CreateUserMessageItem(htmlPrompt));

            await foreach (StreamingResponseUpdate update in client.CreateResponseStreamingAsync(htmlOptions))
            {
                if (update is StreamingResponseOutputTextDeltaUpdate delta)
                {
                    htmlResponse.Append(delta.Delta);
                }
            }

            var htmlContent = htmlResponse.ToString();
            
            // Clean up response if it contains markdown code blocks
            if (htmlContent.StartsWith("```html"))
            {
                htmlContent = htmlContent[7..];
            }
            else if (htmlContent.StartsWith("```"))
            {
                htmlContent = htmlContent[3..];
            }
            
            if (htmlContent.EndsWith("```"))
            {
                htmlContent = htmlContent[..^3];
            }
            
            htmlContent = htmlContent.Trim();

            await File.WriteAllTextAsync(htmlOutputPath, htmlContent);
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Error generating HTML: {ex.Message}[/]");
        }
    });

// Display completion panel
var panel = new Panel(
    new Markup($"[green]Research completed successfully![/]\n\n" +
               $"[white]Topic:[/] [cyan]{researchTopic}[/]\n" +
               $"[white]Depth:[/] [yellow]{researchDepth}[/]\n" +
               $"[white]Iterations:[/] [yellow]{maxIterations}[/]\n" +
               $"[white]Markdown:[/] [blue]{fileName}[/]\n" +
               $"[white]HTML:[/] [blue]{htmlFileName}[/]"))
{
    Header = new PanelHeader("[green] Research Complete [/]"),
    Border = BoxBorder.Rounded,
    BorderStyle = Style.Parse("green"),
    Padding = new Padding(2, 1)
};

AnsiConsole.WriteLine();
AnsiConsole.Write(panel);

AnsiConsole.WriteLine();
AnsiConsole.MarkupLine($"[grey]Research saved to: {outputPath}[/]");
AnsiConsole.MarkupLine($"[grey]HTML report saved to: {htmlOutputPath}[/]");
