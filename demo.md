# 🏦 SA Banking App Demo Guide

## Quick Start Demo

### 1. Start the Application
```bash
./run.sh
# or
dotnet run
```

### 2. Access the Web Interface
- **Main App**: http://localhost:5000
- **API Docs**: http://localhost:5000/swagger

### 3. Demo Walkthrough

#### Step 1: Create Your First Account
1. Open http://localhost:5000 in your browser
2. Click the "Create Account" tab
3. Fill in the form:
   - **Name**: John Doe
   - **PIN**: 1234
   - **Account Type**: Savings Account
4. Click "Create Account"
5. You'll be automatically logged in and see your dashboard!

#### Step 2: Explore the Dashboard
- **Balance Card**: Shows your current balance with beautiful styling
- **Quick Actions**: Deposit, Withdraw, Transfer buttons
- **Account Details**: Your account information
- **Recent Transactions**: Transaction history (empty initially)

#### Step 3: Make a Deposit
1. Click the "Deposit" button in Quick Actions
2. Enter amount: 1000.00
3. Enter PIN: 1234
4. Click "Deposit"
5. Watch your balance update in real-time!

#### Step 4: Try Other Features
- **Withdraw**: Take out some money
- **Transfer**: Create another account and transfer between them
- **Change PIN**: Update your security
- **Convert Account Type**: Switch to Cheque or Business account

#### Step 5: Admin Panel
1. Click "Admin Panel" button
2. Enter password: `admin123`
3. **View All Accounts**: See all accounts in the system
4. **Create Backup**: Make a system backup
5. **Restore Backup**: Restore from previous backup

### 4. API Testing with Swagger

1. Go to http://localhost:5000/swagger
2. Try the endpoints:
   - `POST /api/banking/accounts` - Create account
   - `POST /api/banking/login` - Login
   - `GET /api/banking/accounts` - List all accounts

### 5. Mobile Testing

1. Open the app on your mobile device
2. Navigate to http://YOUR_IP:5000
3. Test the responsive design
4. Try touch interactions

## 🎯 Key Features to Test

### ✅ Core Banking
- [ ] Account creation
- [ ] Login/logout
- [ ] Deposits and withdrawals
- [ ] Transfers between accounts
- [ ] PIN changes
- [ ] Account type conversion

### ✅ User Experience
- [ ] Responsive design
- [ ] Smooth animations
- [ ] Toast notifications
- [ ] Modal dialogs
- [ ] Tab navigation

### ✅ Admin Features
- [ ] View all accounts
- [ ] Backup creation
- [ ] Backup restoration
- [ ] System monitoring

### ✅ Technical Features
- [ ] API endpoints
- [ ] Error handling
- [ ] Data persistence
- [ ] Real-time updates

## 🐛 Troubleshooting

### Common Issues
1. **Port already in use**: Change ports in `Properties/launchSettings.json`
2. **Build errors**: Run `dotnet restore` and `dotnet build`
3. **Frontend not loading**: Check browser console for errors
4. **API not responding**: Verify the application is running

### Debug Mode
- Check browser console (F12)
- Monitor network requests
- Review API responses
- Check application logs

## 🚀 Next Steps

After testing the demo:
1. **Customize**: Modify colors, fonts, or layout
2. **Extend**: Add new features like loans or investments
3. **Deploy**: Host on Azure, AWS, or other cloud platforms
4. **Scale**: Add database, authentication, or microservices

---

**Happy Banking! 🎉** 