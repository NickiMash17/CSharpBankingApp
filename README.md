# NexusBank 🏦

> *Because traditional banking UX makes me want to throw my laptop out the window* - [@NickiMash17](https://github.com/NickiMash17)

A rebellion against terrible banking interfaces, built with C# and way too much caffeine. This isn't just another CRUD app—it's what happens when you get tired of banks treating their digital presence like it's still 1999.

## 🚀 TL;DR - Get It Running

**Live Demo**: [https://nexusbank-app.azurewebsites.net](https://nexusbank-app.azurewebsites.net)

**Local Setup** (because you're probably impatient like me):
```bash
git clone https://github.com/NickiMash17/CSharpBankingApp.git
cd CSharpBankingApp
docker-compose up --build -d
# ☕ Grab coffee while Docker does its thing
# 🎉 Open http://localhost:8080
```

## 🎯 The Story Behind This

I got fed up with clunky banking apps that look like they were designed by committee in 2003. So I built what I actually wanted to use—clean, fast, and doesn't make you want to rage-quit every transaction.

**What makes this different:**
- No 47-step verification process for checking your balance
- Actually works on mobile (revolutionary, I know)
- Visual feedback that doesn't suck
- PIN system that remembers humans have fat fingers sometimes

## ✨ What It Actually Does

### The Banking Stuff
- **Multi-Account Support**: Savings, Cheque, Business accounts with different perks
- **Smart Transactions**: Deposit, withdraw, transfer without the drama
- **Interest That Actually Exists**: Unlike my real bank account
- **Account Morphing**: Convert between account types (because life changes)
- **Transaction Archaeology**: Dig through your spending history with filters

### Account Flavors 🍦

| Type | Interest | Overdraft | Best For |
|------|----------|-----------|----------|
| **Savings** | 2.5% | Nope | Future yacht fund |
| **Cheque** | 0.5% | R200 | Adulting expenses |
| **Business** | 1.0% | R500 | Side hustle empire |

### The UI Experience
- **Glassmorphism Design**: Because flat design is so 2010s
- **Responsive Everything**: Works on your phone, tablet, or that ancient laptop
- **Real-time Updates**: Balance changes instantly (like my mood)
- **Toast Notifications**: Friendly feedback without being annoying
- **Modal Magic**: Clean forms that don't hijack your entire screen

## 🛠️ Built With Love And

**Backend Arsenal:**
- **.NET 8.0** - The new hotness
- **ASP.NET Core** - Web API that doesn't hate you
- **Custom JSON Wizardry** - Because serialization should just work
- **File-based Storage** - Sometimes simple is better

**Frontend Magic:**
- **Vanilla JS** - No framework bloat, just pure speed
- **Modern CSS** - Grid, Flexbox, Custom Properties FTW
- **Font Awesome** - Icons that don't look like hieroglyphs
- **Space Grotesk** - Typography that sparks joy

**DevOps Reality:**
- **Docker** - "It works on my machine" problem solved
- **Azure** - Cloud hosting that doesn't break the bank (pun intended)

## 🏗️ Architecture Deep Dive

```
CSharpBankingApp/
├── Controllers/           # Where HTTP requests come to party
│   └── BankingController.cs
├── Models/               # Data structures that make sense
│   ├── Account.cs        # Your digital piggy bank
│   ├── Bank.cs           # The brain of the operation
│   └── Transaction.cs    # Money movement tracker
├── Services/             # Business logic fortress
│   └── BankService.cs
├── wwwroot/             # Frontend goodness
│   ├── index.html       # The face of the operation
│   ├── styles.css       # Where design magic happens
│   └── script.js        # UI choreography
├── backups/             # Your safety net
├── Dockerfile           # Container recipe
└── docker-compose.yml   # Orchestration symphony
```

## 🎮 How To Use This Thing

### Step 1: Create Your Digital Identity
1. Hit the "Create Account" tab
2. Tell us your name (we promise not to sell it)
3. Pick a PIN (not your birthday, please)
4. Choose your account personality
5. Click create and watch the magic happen

### Step 2: Move Money Around
1. Login (demo mode accepts any credentials—we're trusting like that)
2. Use the shiny action buttons
3. Enter amounts and PIN
4. Confirm and feel financially empowered

### Step 3: Channel Your Inner Admin
- Password: `admin123` (I know, super secure 😅)
- View all accounts like a digital overlord
- Create backups because paranoia is healthy
- Restore when things go sideways

## 🔌 API Playground

| What You Want | How To Get It | Why You'd Want It |
|---------------|---------------|-------------------|
| `GET /api/banking/accounts` | All accounts | Stalking financial status |
| `GET /api/banking/accounts/{id}` | Specific account | Targeted snooping |
| `POST /api/banking/accounts` | Create account | Birth new financial identity |
| `POST /api/banking/deposit` | Add money | Simulate being rich |
| `POST /api/banking/withdraw` | Remove money | Simulate real life |
| `POST /api/banking/transfer` | Move money | Play financial tetris |

Full swagger docs at `/swagger` because documentation matters.

## 🚀 Deployment Options

### For Local Heroes
```bash
# Docker route (recommended)
docker-compose up --build -d

# Traditional route
dotnet restore && dotnet run
```

### For Cloud Enthusiasts
- **Azure**: Already live and loving it
- **Heroku**: `git push heroku main` and you're famous
- **Railway**: Connect GitHub, let robots do the work
- **Digital Ocean**: App Platform makes it stupid simple

### For Network Sharing
Find your local IP and share `http://YOUR_IP:8080` with friends who appreciate good UX.

## 🛡️ Security Philosophy

Look, this is a demo app, not Fort Knox. But I still implemented:
- PIN authentication (because passwords are so last century)
- Input validation (preventing chaos)
- Secure error messages (no accidental data leaks)
- Session management (keeping track of who's who)

**Real talk**: Don't use this for actual money. That would be... unwise.

## 🐛 When Things Go Wrong

**Docker being difficult?**
```bash
docker-compose down
docker system prune -f
docker-compose up --build -d
```

**Frontend acting up?**
- Check browser console (F12 is your friend)
- Clear cache and cookies
- Try incognito mode

**Backend throwing tantrums?**
- Check `bankdata.json` permissions
- Look at container logs: `docker logs nexusbank`
- Restart everything and hope for the best

## 🤝 Want to Contribute?

I'd love that! Here's how:

1. Fork this beauty
2. Create a branch with a name that sparks joy
3. Write code that makes you proud
4. Test it like your reputation depends on it
5. Send a PR with a description that tells a story

**Coding Style**: Clean, commented, and considerate. If you wouldn't want to debug it at 2 AM, don't commit it.

## 🌟 Future Dreams

- [ ] Dark mode (for the night owls)
- [ ] Mobile app (React Native maybe?)
- [ ] Real-time notifications
- [ ] Biometric authentication
- [ ] AI-powered spending insights
- [ ] Cryptocurrency integration (because why not?)

## 🎨 Design Credits

Inspired by every banking app that ever frustrated me, and built to be the opposite of that experience. Special thanks to:
- Coffee shops with good WiFi
- Spotify playlists that fuel coding sessions
- The Stack Overflow community (you know why)

## 📝 License & Legal Stuff

MIT License - do whatever makes you happy with this code.

**Disclaimer**: This is a demonstration project. Please don't try to replace your actual bank with this. Your financial advisor would not approve.

---

*Built with ❤️ and excessive amounts of determination by [Nicolette Mashaba](https://github.com/NickiMash17)*

**P.S.** - If you actually read this entire README, you deserve a cookie. Unfortunately, this app doesn't dispense cookies yet. Maybe v2.0? 🍪