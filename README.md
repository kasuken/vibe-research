# Vibe Research

Deep Research CLI powered by OpenAI Reasoning. Conducts iterative research on any topic and generates comprehensive markdown and HTML reports.

## Features

- 🔬 **Deep Research**: Iterative research with multiple phases for comprehensive coverage
- 🧠 **AI Reasoning**: Uses OpenAI's reasoning models (GPT-5.1) with configurable effort levels
- 📄 **Markdown Output**: Clean, well-structured markdown reports saved to `docs/`
- 🌐 **HTML Reports**: Professional, standalone HTML pages saved to `html/`
- 🎨 **Beautiful CLI**: Spectre.Console powered interface with progress indicators
- 🤖 **GitHub Actions**: Trigger research via Issues or workflow dispatch
- 📝 **Auto-commit**: Results are automatically committed to the repository

## Quick Start

### Option 1: GitHub Issue (Easiest)

1. **Create a new Issue** with the `research` label
2. **Title** = Your research topic (e.g., "Quantum Computing Applications")
3. **Body** (optional) = Include "Deep Dive" or "Quick Overview" to set depth
4. **Wait** for the action to complete
5. **Check the comments** for links to the generated files

The action will:
- Start processing automatically
- Comment on the issue with progress
- Commit results to `docs/` and `html/` folders
- Add a final comment with links to the files
- Close the issue with `research-complete` label

### Option 2: Workflow Dispatch (Manual)

1. Go to **Actions** → **Vibe Research**
2. Click **Run workflow**
3. Enter your research topic and select depth
4. Download artifacts or check `docs/` and `html/` folders

### Option 3: Local CLI

1. **Clone the repository**
   ```bash
   git clone https://github.com/your-org/VibeResearch.git
   cd VibeResearch
   ```

2. **Configure API Key** (choose one method):
   
   **Option A: Environment Variable (recommended)**
   ```powershell
   $env:OPENAI_API_KEY = "sk-your-key-here"
   ```
   ```bash
   export OPENAI_API_KEY="sk-your-key-here"
   ```

   **Option B: appsettings.json**
   ```bash
   cp VibeResearch.CLI/appsettings.template.json VibeResearch.CLI/appsettings.json
   # Edit appsettings.json and add your API key
   ```

3. **Run the application**
   ```bash
   cd VibeResearch.CLI
   dotnet run
   ```

4. **Follow the prompts** to enter your research topic and select depth

## Research Depth Options

| Depth | Iterations | Reasoning Effort | Best For |
|-------|------------|------------------|----------|
| Quick Overview | 2 | Low | Fast summaries |
| Standard Research | 4 | Medium | Balanced research |
| Deep Dive | 6 | High | Comprehensive analysis |

## GitHub Actions Setup

### 1. Add OpenAI API Key Secret

1. Go to repository **Settings** → **Secrets and variables** → **Actions**
2. Click **New repository secret**
3. Name: `OPENAI_API_KEY`
4. Value: Your OpenAI API key

### 2. Create the `research` Label

1. Go to **Issues** → **Labels**
2. Create a new label named `research`
3. (Optional) Create `research-complete` label for completed issues

### Environment Variables (CI Mode)

| Variable | Required | Description |
|----------|----------|-------------|
| `OPENAI_API_KEY` | Yes | Your OpenAI API key |
| `VIBE_TOPIC` | Yes | The research topic |
| `VIBE_DEPTH` | No | `Quick Overview`, `Standard Research` (default), or `Deep Dive` |
| `VIBE_OUTPUT_DIR` | No | Output directory (default: current directory) |

## Output Structure

```
VibeResearch/
├── docs/                    # Markdown reports
│   └── research_*.md
├── html/                    # HTML reports
│   └── research_*.html
└── ...
```

## Requirements

- .NET 10.0 SDK
- OpenAI API key with access to reasoning models

## License

MIT


