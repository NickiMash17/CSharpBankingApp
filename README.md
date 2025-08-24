# SA Banking App - Enhanced with Web Frontend

A modern, feature-rich banking application built with C# and ASP.NET Core, featuring both a console interface and a beautiful web frontend.

## Features

### Core Banking Features
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

### Web Frontend Features
- **Modern UI/UX**: Beautiful, responsive design with glassmorphism effects
- **Real-time Updates**: Live balance and transaction updates
- **Mobile Responsive**: Optimized for all device sizes
- **Interactive Dashboard**: Intuitive banking interface
- **Modal-based Operations**: Clean, focused transaction forms
- **Toast Notifications**: User-friendly feedback system

### Admin Features
- **Account Overview**: View all accounts in the system
- **Backup Management**: Create and restore system backups
- **System Monitoring**: Admin panel for system administration

## Technology Stack

### Backend
- **.NET 8.0**: Latest .NET framework
- **ASP.NET Core**: Modern web framework
- **Web API**: RESTful API endpoints
- **JSON Serialization**: Efficient data handling
- **File-based Storage**: Persistent data storage with backup system

### Frontend
- **HTML5**: Semantic markup
- **CSS3**: Modern styling with CSS Grid and Flexbox
- **JavaScript (ES6+)**: Modern JavaScript features
- **Responsive Design**: Mobile-first approach
- **Font Awesome**: Beautiful icons
- **Google Fonts**: Inter font family

## Project Structure

```
CSharpBankingApp/
├── Controllers/           # API Controllers
│   └── BankingController.cs
├── Models/               # Data Models
│   ├── Account.cs
│   ├── Bank.cs
│   └── Transaction.cs
├── Services/             # Business Logic
│   └── BankService.cs
├── wwwroot/             # Frontend Assets
│   ├── index.html       # Main HTML file
│   ├── styles.css       # CSS styling
│   └── script.js        # JavaScript functionality
├── backups/             # System backups
├── Program.cs           # Application entry point
├── CSharpBankingApp.csproj
└── README.md
```

## Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Modern web browser
- Code editor (VS Code, Visual Studio, etc.)

### Installation & Running

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
   - Open your browser and navigate to: `https://localhost:5001` or `http://localhost:5000`
   - The API documentation is available at: `https://localhost:5001/swagger`

### Default Admin Credentials
- **Username**: Any existing account name
- **Admin Password**: `admin123`

## API Endpoints

### Authentication
- `POST /api/banking/login` - Login to existing account
- `POST /api/banking/accounts` - Create new account

### Account Management
- `GET /api/banking/accounts` - Get all accounts (admin)
- `GET /api/banking/accounts/{id}` - Get specific account
- `POST /api/banking/accounts/{id}/change-pin` - Change PIN
- `POST /api/banking/accounts/{id}/convert-type` - Convert account type

### Transactions
- `POST /api/banking/accounts/{id}/deposit` - Make deposit
- `POST /api/banking/accounts/{id}/withdraw` - Make withdrawal
- `POST /api/banking/transfer` - Transfer between accounts

### Admin Operations
- `GET /api/banking/backups` - List available backups
- `POST /api/banking/backups` - Create new backup
- `POST /api/banking/backups/restore` - Restore from backup

## Usage Examples

### Creating an Account
1. Navigate to the "Create Account" tab
2. Enter your full name
3. Choose a 4-digit PIN
4. Select account type
5. Click "Create Account"

### Making Transactions
1. Login to your account
2. Use the Quick Actions buttons for deposits/withdrawals
3. Enter amount and PIN
4. Confirm transaction

### Admin Operations
1. Click "Admin Panel" button
2. Enter admin password: `admin123`
3. Choose operation (view accounts, backup, restore)

## Security Features

- **PIN Authentication**: 4-digit PIN required for all transactions
- **Input Validation**: Comprehensive input sanitization
- **Error Handling**: Secure error messages without information leakage
- **Session Management**: Secure user session handling

## Data Persistence

- **JSON Storage**: Account data stored in `bankdata.json`
- **Automatic Backups**: System creates backups every 5 minutes
- **Manual Backups**: Admin can create backups on demand
- **Backup Restoration**: Ability to restore from any available backup

## Browser Compatibility

- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+
- Mobile browsers (iOS Safari, Chrome Mobile)

## Development

### Adding New Features
1. Extend the `Bank` model in `Models/Bank.cs`
2. Add corresponding methods to `BankService.cs`
3. Create API endpoints in `BankingController.cs`
4. Update the frontend in `wwwroot/` files

### Styling
- CSS uses modern features like CSS Grid, Flexbox, and CSS Variables
- Responsive design with mobile-first approach
- Glassmorphism and modern UI patterns

### JavaScript
- ES6+ features for modern browser support
- Async/await for API calls
- Event-driven architecture
- Modular function organization

## Troubleshooting

### Common Issues
1. **Port already in use**: Change ports in `Properties/launchSettings.json`
2. **Data not persisting**: Check file permissions for `bankdata.json`
3. **Frontend not loading**: Ensure `wwwroot` folder is in the correct location

### Performance
- The application uses efficient JSON serialization
- File I/O operations are optimized with proper locking
- Frontend assets are optimized for fast loading

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Test thoroughly
5. Submit a pull request

## License

This project is open source and available under the MIT License.

## Support

For issues and questions:
1. Check the troubleshooting section
2. Review the API documentation at `/swagger`
3. Check browser console for JavaScript errors
4. Verify .NET version compatibility

---

**Enjoy your enhanced banking experience! 🏦✨** 