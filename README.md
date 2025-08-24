# NexusBank - Revolutionary Digital Banking Experience

A modern, feature-rich banking application built with C# and ASP.NET Core, featuring a beautiful, professional web frontend with glassmorphism design and comprehensive banking operations.

## 🚀 Local Access

**Your NexusBank is running locally and accessible at:** `http://localhost:8080`

> **Note:** This URL is only accessible on your local machine. To make it accessible to others, see the [Deployment Options](#deployment-options) section below.

## 🌐 Azure Deployment Status

**Your NexusBank is now deployed to Azure!** 🎉

- **Live URL**: `https://nexusbank-app.azurewebsites.net`
- **Status**: ✅ Successfully deployed
- **Resource Group**: `nexusbank-rg`
- **App Service Plan**: `nexusbank-plan`

## ✨ Features

### 🏦 Core Banking Features
- **Account Management**: Create and manage different types of accounts
- **Multiple Account Types**:
  - Savings Account (2.5% interest, no overdraft)
  - Cheque Account (0.5% interest, R200 overdraft)
  - Business Account (1% interest, R500 overdraft)
- **Transaction Operations**: Deposit, withdraw, and transfer funds
- **PIN Security**: Secure 4-digit PIN authentication
- **Interest Calculation**: Automatic interest calculation and application
- **Account Conversion**: Convert between different account types
- **Transaction History**: View detailed transaction records with filtering

### 🎨 Modern Web Frontend
- **Professional UI/UX**: Clean, modern design with glassmorphism effects
- **Responsive Dashboard**: Beautiful banking interface optimized for all devices
- **Real-time Updates**: Live balance and transaction updates
- **Interactive Elements**: Smooth animations and transitions
- **Modal-based Operations**: Clean, focused transaction forms
- **Toast Notifications**: User-friendly feedback system

### 🔧 Admin Features
- **Account Overview**: View all accounts in the system
- **Backup Management**: Create and restore system backups
- **System Monitoring**: Admin panel for system administration

## 🛠️ Technology Stack

### Backend
- **.NET 8.0**: Latest .NET framework
- **ASP.NET Core**: Modern web framework
- **Web API**: RESTful API endpoints
- **JSON Serialization**: Efficient data handling with custom converters
- **File-based Storage**: Persistent data storage with backup system

### Frontend
- **HTML5**: Semantic markup
- **CSS3**: Modern styling with CSS Grid, Flexbox, and CSS Variables
- **JavaScript (ES6+)**: Modern JavaScript features and async operations
- **Responsive Design**: Mobile-first approach
- **Font Awesome**: Beautiful icons
- **Google Fonts**: Space Grotesk and Inter font families

### Deployment
- **Docker**: Containerized deployment
- **Docker Compose**: Easy service orchestration
- **Port Mapping**: Accessible on localhost:8080

## 📁 Project Structure

```
CSharpBankingApp/
├── Controllers/           # API Controllers
│   └── BankingController.cs
├── Models/               # Data Models
│   ├── Account.cs        # Account model with custom JSON converter
│   ├── Bank.cs           # Bank service with JSON handling
│   └── Transaction.cs    # Transaction model
├── Services/             # Business Logic
│   └── BankService.cs
├── wwwroot/             # Frontend Assets
│   ├── index.html       # Main HTML file
│   ├── styles.css       # Modern CSS styling
│   └── script.js        # JavaScript functionality
├── backups/             # System backups
├── Dockerfile           # Docker configuration
├── docker-compose.yml   # Docker Compose setup
├── Program.cs           # Application entry point
├── CSharpBankingApp.csproj
└── README.md
```

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Docker and Docker Compose
- Modern web browser
- Code editor (VS Code, Visual Studio, etc.)

### 🐳 Docker Deployment (Local Development)

1. **Clone the repository**
   ```bash
   git clone https://github.com/NickiMash17/CSharpBankingApp.git
   cd CSharpBankingApp
   ```

2. **Build and run with Docker**
   ```bash
   docker-compose up --build -d
   ```

3. **Access your banking app locally**
   - Open your browser and go to: `http://localhost:8080`
   - The app is now running on your local machine!

## 🌐 Deployment Options

### **Local Network Access (Same WiFi)**
If others are on the same network as you:
```bash
# Find your local IP address
ip addr show

# Others can access: http://YOUR_LOCAL_IP:8080
# Example: http://192.168.1.100:8080
```

### **Public Cloud Deployment**
To make your app accessible worldwide:

#### **Heroku (Recommended for public access)**
```bash
# Install Heroku CLI
curl https://cli-assets.heroku.com/install.sh | sh

# Deploy to Heroku
heroku create your-nexusbank-app
git push heroku main

# Your app will be at: https://your-nexusbank-app.herokuapp.com
```

#### **Railway**
- Connect your GitHub repo to Railway
- Automatic deployment from your repository
- Free tier available

#### **Azure App Service**
- Use the included `azure-deploy.yml` workflow
- Deploy directly from GitHub

### 🖥️ Local Development

1. **Clone or download the project**
   ```bash
   cd CSharpBankingApp
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Run the application**
   ```bash
   dotnet run
   ```

4. **Access the web application**
   - Open your browser and navigate to: `http://localhost:5000`
   - The API documentation is available at: `http://localhost:5000/swagger`

## 🎯 Usage Examples

### Creating an Account
1. Navigate to the "Create Account" tab
2. Enter your full name
3. Choose a 4-digit PIN
4. Select account type
5. Click "Create Account"

### Making Transactions
1. Login to your account (demo mode - any credentials work)
2. Use the Quick Actions buttons for deposits/withdrawals
3. Enter amount and PIN
4. Confirm transaction

### Admin Operations
1. Click "Admin Panel" button
2. Enter admin password: `admin123`
3. Choose operation (view accounts, backup, restore)

## 🔒 Security Features

- **PIN Authentication**: 4-digit PIN required for all transactions
- **Input Validation**: Comprehensive input sanitization
- **Error Handling**: Secure error messages without information leakage
- **Session Management**: Secure user session handling

## 💾 Data Persistence

- **JSON Storage**: Account data stored in `bankdata.json`
- **Automatic Backups**: System creates backups every 5 minutes
- **Manual Backups**: Admin can create backups on demand
- **Backup Restoration**: Ability to restore from any available backup

## 🌐 Browser Compatibility

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+
- Mobile browsers (iOS Safari, Chrome Mobile)

## 🐳 Docker Commands

### Start the application
```bash
docker-compose up -d
```

### Stop the application
```bash
docker-compose down
```

### View logs
```bash
docker logs csharpbankingapp_nexusbank_1
```

### Rebuild and restart
```bash
docker-compose up --build -d
```

## 🔧 Development

### Adding New Features
1. Extend the `Bank` model in `Models/Bank.cs`
2. Add corresponding methods to `BankService.cs`
3. Create API endpoints in `BankingController.cs`
4. Update the frontend in `wwwroot/` files

### Styling
- CSS uses modern features like CSS Grid, Flexbox, and CSS Variables
- Responsive design with mobile-first approach
- Glassmorphism and modern UI patterns
- Professional color scheme and typography

### JavaScript
- ES6+ features for modern browser support
- Async/await for API calls
- Event-driven architecture
- Modular function organization

## 🚨 Troubleshooting

### Common Issues
1. **Port already in use**: Change ports in `docker-compose.yml`
2. **Data not persisting**: Check file permissions for `bankdata.json`
3. **Frontend not loading**: Ensure `wwwroot` folder is in the correct location
4. **Docker issues**: Use `docker-compose down` then `docker-compose up --build -d`

### Performance
- The application uses efficient JSON serialization with custom converters
- File I/O operations are optimized with proper locking
- Frontend assets are optimized for fast loading
- Docker containerization ensures consistent performance

## 📝 API Endpoints

- `GET /api/banking/accounts` - Get all accounts
- `GET /api/banking/accounts/{id}` - Get specific account
- `POST /api/banking/accounts` - Create new account
- `POST /api/banking/deposit` - Deposit funds
- `POST /api/banking/withdraw` - Withdraw funds
- `POST /api/banking/transfer` - Transfer between accounts
- `GET /api/banking/backups` - List available backups
- `POST /api/banking/backups` - Create new backup
- `POST /api/banking/backups/restore` - Restore from backup

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## 📄 License

This project is open source and available under the MIT License.

## 🆘 Support

For issues and questions:
1. Check the troubleshooting section
2. Review the API documentation at `/swagger`
3. Check browser console for JavaScript errors
4. Verify .NET version compatibility
5. Check Docker container logs

---

**🎉 Your NexusBank is now running locally and ready for development! 🏦✨**

**To share with others:** Deploy to a cloud platform using the options above. 