# ConsoleJobs 💼

## The Story

I got tired of switching between job platforms, copy-pasting filters, and context-switching between tabs. As someone who loves working in the console, I thought: *Why not build a job search tool that lives right in the terminal?* 

Then I added AI into the mix. Now you can search jobs, get intelligent recommendations, and let Llama analyze opportunities—all without leaving your console.

---

## What Makes ConsoleJobs Different?

✨ **AI-Powered Job Recommendations** - Instead of manually filtering through hundreds of listings, Llama AI understands your preferences and recommends the best matches for you.

🚀 **Offline AI with Docker** - Run Llama locally in a container. Your job search data never leaves your machine. No API costs. No cloud dependencies.

⚡ **Real Job Data** - Integrated with RapidAPI's JSearch to fetch real job listings worldwide, not just cached data.

---

## Quick Start

### Prerequisites
- .NET 10.0 SDK
- Docker & Docker Desktop running
- RapidAPI account (free tier works) with JSearch API key

### 1. Clone & Setup

```bash
git clone https://github.com/yourusername/ConsoleJobs.git
cd ConsoleJobs
```

### 2. Configure Your API Key

Edit `appsettings.json`:
```json
{
  "RapidApi": {
    "ApiKey": "your-rapidapi-key-here"
  }
}
```

Get your API key from [RapidAPI JSearch](https://rapidapi.com/laxarrudev/api/jsearch)

### 3. Start Llama with Docker

```bash
docker-compose up -d
```

Wait 15 seconds, then download the Mistral model (~5GB, one-time):
```bash
docker exec llama-ai ollama pull mistral
```

Verify it worked:
```bash
docker exec llama-ai ollama list
```

### 4. Run the App

```bash
dotnet run
```

---

## How It Works

1. **Search** - Enter job keywords, location, and date filters
2. **Fetch** - The app pulls real job listings from JSearch API
3. **Analyze** - Llama AI recommends the best matches based on your preferences
4. **Explore** - Get detailed AI analysis on any job that interests you

---

## Tech Stack

- **C# / .NET 10.0** - Core application
- **RapidAPI JSearch** - Real job data
- **Ollama / Mistral** - Local AI inference
- **Docker** - Containerization

---

## Future Ideas

- Export job recommendations to PDF
- Save search history & preferences
- Multi-language support
- Support for different Llama models (Llama 2, Neural Chat)

---

## License

MIT - Feel free to use and modify for your own projects.

---

**Have an idea or found a bug?** Open an issue or submit a PR!
