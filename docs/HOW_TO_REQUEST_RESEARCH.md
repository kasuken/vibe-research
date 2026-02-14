# How to Request Research

This guide explains how to submit research requests using GitHub Issues.

## Using the Issue Template

1. **Navigate to Issues**
   - Go to the [Issues tab](../../issues)
   - Click the green "New issue" button

2. **Select "Research Request" template**
   - You'll see the "🔬 Research Request" option
   - Click "Get started"

3. **Fill out the form**

   ### Research Topic (Required)
   Enter the main topic you want researched. Be specific and clear.
   
   **Examples:**
   - "Quantum Computing in Drug Discovery"
   - "Modern Web Development Frameworks Comparison"
   - "Climate Change Impact on Agriculture"
   - "Machine Learning Ethics and Bias"

   ### Research Depth (Required)
   Choose how comprehensive the research should be:
   
   | Option | Iterations | Time | Best For |
   |--------|-----------|------|----------|
   | **Quick Overview** | 2 | ~2-3 min | Fast summaries, initial exploration |
   | **Standard Research** | 4 | ~5-7 min | Balanced depth, recommended for most topics |
   | **Deep Dive** | 6 | ~10-15 min | Comprehensive analysis, complex topics |

   ### Additional Context (Optional)
   Provide guidance to focus the research:
   - Time periods to focus on
   - Specific subtopics or questions
   - Areas to emphasize or avoid
   
   **Example:**
   ```
   - Focus on developments from 2024-2026
   - Include practical applications in healthcare
   - Compare traditional vs quantum approaches
   ```

   ### Specific Requirements (Optional)
   Any special formatting or content requirements:
   - Citation style preferences
   - Technical depth
   - Code examples needed
   - Comparison tables

4. **Acknowledge and Submit**
   - Check the acknowledgment boxes
   - Click "Submit new issue"

5. **Wait for Results**
   - The action will start automatically
   - You'll receive a comment when processing starts
   - Results are committed to `docs/` and `html/` folders
   - A final comment provides links to the generated files
   - The issue will be closed with the `research-complete` label

## What Gets Generated

After submission, you'll receive:

1. **Markdown Report** (`docs/research_*.md`)
   - Clean, formatted text
   - Easy to read and edit
   - Good for version control

2. **HTML Report** (`html/research_*.html`)
   - Professional web page
   - Embedded styling
   - Can be viewed in any browser

## Tips for Better Research

### ✅ Do:
- Be specific with your topic
- Provide relevant context
- Use proper research depth for your needs
- Include timeframes if relevant

### ❌ Don't:
- Submit overly broad topics without context
- Request multiple unrelated topics in one issue
- Expect real-time information (model knowledge cutoff applies)

## Examples

### Example 1: Technology Research
```yaml
Topic: "Rust vs Go for Backend Development"
Depth: Standard Research
Context:
  - Focus on performance, concurrency, and developer experience
  - Include real-world case studies
  - Compare ecosystem maturity
```

### Example 2: Scientific Research
```yaml
Topic: "CRISPR Gene Editing Applications in 2025"
Depth: Deep Dive
Context:
  - Focus on medical applications
  - Include ethical considerations
  - Recent clinical trials and results
Requirements:
  - Include citations and references
  - Technical details on mechanisms
```

### Example 3: Quick Investigation
```yaml
Topic: "GraphQL vs REST API"
Depth: Quick Overview
Context:
  - High-level comparison
  - Use cases for each
```

## Troubleshooting

**Issue not triggering the workflow?**
- Ensure the issue has the `research` label (auto-applied by template)
- Check that `OPENAI_API_KEY` is set in repository secrets

**Action failing?**
- Check the Actions tab for error logs
- Verify API key has sufficient credits
- Ensure the topic is appropriate and not blocked by content policies

**Need help?**
- Check the [README](../README.md)
- Open a discussion in [Discussions](../../discussions)

## Advanced: Manual Trigger

You can also trigger research manually via workflow dispatch:
1. Go to **Actions** → **Vibe Research**
2. Click **Run workflow**
3. Enter topic and select depth
4. Results will be committed to the repository

