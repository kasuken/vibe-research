# 🚀 Quick Reference Card

## For Users: Request Research

### Method 1: Issue Template (Easiest)
1. Click: [New Research Request](../../issues/new/choose)
2. Select "🔬 Research Request"
3. Fill: Topic + Depth + Context (optional)
4. Submit & Wait for results in comments

### Method 2: Manual Workflow
1. Go to: Actions → Vibe Research → Run workflow
2. Enter: Topic and Depth
3. Check: `docs/` and `html/` folders for results

---

## Research Depth Quick Guide

| Depth | Time | Iterations | Use When |
|-------|------|------------|----------|
| 🏃 Quick Overview | 2-3 min | 2 | Fast summary needed |
| 📚 Standard Research | 5-7 min | 4 | Most topics (recommended) |
| 🔬 Deep Dive | 10-15 min | 6 | Comprehensive analysis |

---

## Output Locations

- 📝 **Markdown**: `docs/research_*.md`
- 🌐 **HTML**: `html/research_*.html`
- 💾 **Artifacts**: Actions tab (30 days)

---

## For Maintainers: Setup

### Required Secret
```
Settings → Secrets → Actions → New secret
Name: OPENAI_API_KEY
Value: sk-...
```

### Auto-created Labels
- `research` - Applied by template
- `research-complete` - Applied when done

---

## Files Structure

```
VibeResearch/
├── .github/
│   ├── ISSUE_TEMPLATE/
│   │   ├── research_request.yml    # Issue form
│   │   └── config.yml              # Template config
│   └── workflows/
│       └── vibe-research.yml       # GitHub Action
├── docs/                           # Markdown outputs
│   ├── README.md
│   ├── HOW_TO_REQUEST_RESEARCH.md
│   ├── EXAMPLES.md
│   └── QUICK_REFERENCE.md (this file)
├── html/                           # HTML outputs
├── VibeResearch.CLI/              # .NET CLI app
└── README.md                       # Main docs
```

---

## Troubleshooting

| Problem | Solution |
|---------|----------|
| Workflow not starting | Check issue has `research` label |
| Action failing | Verify `OPENAI_API_KEY` secret exists |
| No output files | Check Actions logs for errors |
| API rate limits | Wait or upgrade OpenAI plan |

---

## Quick Links

- 📖 [Full Documentation](../README.md)
- 📝 [How to Request Research](HOW_TO_REQUEST_RESEARCH.md)
- 💡 [Examples](EXAMPLES.md)
- 🐛 [Issues](../../issues)
- 💬 [Discussions](../../discussions)

---

## Environment Variables (for CI)

```bash
OPENAI_API_KEY      # Required: OpenAI API key
VIBE_TOPIC          # Required: Research topic
VIBE_DEPTH          # Optional: Quick Overview|Standard Research|Deep Dive
VIBE_OUTPUT_DIR     # Optional: Output directory path
```

---

**Made with ❤️ using OpenAI Reasoning Models**

