// Global state
let currentUser = null;
let currentAccount = null;
let isLoading = false;
let sessionTimer = null;
let sessionTimeout = 30 * 60 * 1000; // 30 minutes
let sessionStartTime = Date.now();

// API base URL
const API_BASE = '/api/banking';

// Enhanced utility functions
function showToast(message, type = 'info', duration = 4000) {
    const toast = document.getElementById('toast');
    if (toast) {
        toast.textContent = message;
        toast.className = `toast ${type} show`;
        
        // Auto-hide with smooth animation
        setTimeout(() => {
            toast.classList.remove('show');
        }, duration);
    } else {
        // Create toast if it doesn't exist
        createToast(message, type, duration);
    }
}

function createToast(message, type = 'info', duration = 4000) {
    const toast = document.createElement('div');
    toast.className = `notification ${type}`;
    toast.innerHTML = `
        <i class="fas fa-${type === 'success' ? 'check-circle' : type === 'error' ? 'exclamation-circle' : 'info-circle'}"></i>
        <span>${message}</span>
    `;
    
    document.body.appendChild(toast);
    
    // Show toast
    setTimeout(() => toast.classList.add('show'), 100);
    
    // Auto-hide
    setTimeout(() => {
        toast.classList.remove('show');
        setTimeout(() => toast.remove(), 300);
    }, duration);
}

function formatCurrency(amount) {
    return new Intl.NumberFormat('en-ZA', {
        style: 'currency',
        currency: 'ZAR'
    }).format(amount);
}

function formatDate(dateString) {
    return new Date(dateString).toLocaleDateString('en-ZA', {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    });
}

function setLoading(element, loading = true) {
    if (loading) {
        element.classList.add('loading');
        element.disabled = true;
        if (element.tagName === 'BUTTON') {
            const originalText = element.innerHTML;
            element.setAttribute('data-original-text', originalText);
            element.innerHTML = '<i class="fas fa-spinner fa-spin"></i> Loading...';
        }
    } else {
        element.classList.remove('loading');
        element.disabled = false;
        if (element.tagName === 'BUTTON' && element.hasAttribute('data-original-text')) {
            element.innerHTML = element.getAttribute('data-original-text');
            element.removeAttribute('data-original-text');
        }
    }
}

function addRippleEffect(event) {
    const button = event.currentTarget;
    const ripple = document.createElement('span');
    const rect = button.getBoundingClientRect();
    const size = Math.max(rect.width, rect.height);
    const x = event.clientX - rect.left - size / 2;
    const y = event.clientY - rect.top - size / 2;
    
    ripple.style.width = ripple.style.height = size + 'px';
    ripple.style.left = x + 'px';
    ripple.style.top = y + 'px';
    ripple.classList.add('ripple');
    
    button.appendChild(ripple);
    
    setTimeout(() => {
        ripple.remove();
    }, 600);
}

// Enhanced UI Functions
function switchTab(tabName) {
    console.log('Switching to tab:', tabName);
    
    // Hide all tabs
    document.querySelectorAll('.tab-content').forEach(tab => {
        tab.classList.remove('active');
        console.log('Hiding tab:', tab.id);
    });
    
    // Remove active class from all tab buttons
    document.querySelectorAll('.tab-btn').forEach(btn => {
        btn.classList.remove('active');
    });
    
    // Show selected tab
    const targetTab = document.getElementById(tabName + 'Tab');
    if (targetTab) {
        targetTab.classList.add('active');
        console.log('Showing tab:', targetTab.id);
    } else {
        console.error('Tab not found:', tabName + 'Tab');
    }
    
    // Add active class to selected tab button
    event.target.classList.add('active');
}

function showModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.style.display = 'block';
        document.body.style.overflow = 'hidden';
        
        // Focus first input
        const firstInput = modal.querySelector('input, select');
        if (firstInput) {
            firstInput.focus();
        }
    }
}

function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.style.display = 'none';
        document.body.style.overflow = 'auto';
        
        // Reset form
        const form = modal.querySelector('form');
        if (form) {
            form.reset();
        }
    }
}

// Close modal when clicking outside
document.addEventListener('click', (e) => {
    if (e.target.classList.contains('modal')) {
        closeModal(e.target.id);
    }
});

// Enhanced Authentication Functions
async function login(event) {
    event.preventDefault();
    
    const loginId = document.getElementById('loginId').value;
    const password = document.getElementById('loginPassword').value;
    
    if (!loginId || !password) {
        showToast('Please fill in all fields', 'error');
        return;
    }
    
    setLoading(event.target.querySelector('button[type="submit"]'), true);
    
    try {
        const response = await fetch(`${API_BASE}/login`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ loginId, password })
        });
        
        if (response.ok) {
            const userData = await response.json();
            currentUser = userData;
            showDashboard();
            startSessionTimer();
            showToast('Login successful!', 'success');
        } else {
            const error = await response.json();
            showToast(error.message || 'Login failed', 'error');
        }
    } catch (error) {
        showToast('Network error. Please try again.', 'error');
    } finally {
        setLoading(event.target.querySelector('button[type="submit"]'), false);
    }
}

async function register(event) {
    event.preventDefault();
    
    const formData = {
        fullName: document.getElementById('regFullName').value,
        idNumber: document.getElementById('regIdNumber').value,
        email: document.getElementById('regEmail').value,
        phone: document.getElementById('regPhone').value,
        password: document.getElementById('regPassword').value,
        confirmPassword: document.getElementById('regConfirmPassword').value
    };
    
    // Validation
    if (formData.password !== formData.confirmPassword) {
        showToast('Passwords do not match', 'error');
        return;
    }
    
    if (formData.password.length < 8) {
        showToast('Password must be at least 8 characters long', 'error');
        return;
    }
    
    setLoading(event.target.querySelector('button[type="submit"]'), true);
    
    try {
        const response = await fetch(`${API_BASE}/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formData)
        });
        
        if (response.ok) {
            showToast('Registration successful! Please login.', 'success');
            switchTab('login');
            event.target.reset();
        } else {
            const error = await response.json();
            showToast(error.message || 'Registration failed', 'error');
        }
    } catch (error) {
        showToast('Network error. Please try again.', 'error');
    } finally {
        setLoading(event.target.querySelector('button[type="submit"]'), false);
    }
}

// Enhanced Banking Functions
async function processTransfer() {
    const form = document.getElementById('transferForm');
    const formData = new FormData(form);
    
    const transferData = {
        fromAccount: formData.get('transferFrom'),
        toAccount: formData.get('transferTo'),
        amount: parseFloat(formData.get('transferAmount')),
        reference: formData.get('transferReference')
    };
    
    if (!transferData.fromAccount || !transferData.toAccount || !transferData.amount || !transferData.reference) {
        showToast('Please fill in all fields', 'error');
        return;
    }
    
    if (transferData.amount <= 0) {
        showToast('Amount must be greater than zero', 'error');
        return;
    }
    
    const submitBtn = form.querySelector('button[type="submit"]') || document.querySelector('#transferModal .btn-primary');
    setLoading(submitBtn, true);
    
    try {
        const response = await fetch(`${API_BASE}/transfer`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(transferData)
        });
        
        if (response.ok) {
            const result = await response.json();
            showToast('Transfer successful!', 'success');
            closeModal('transferModal');
            updateDashboard();
            form.reset();
        } else {
            const error = await response.json();
            showToast(error.message || 'Transfer failed', 'error');
        }
    } catch (error) {
        showToast('Network error. Please try again.', 'error');
    } finally {
        setLoading(submitBtn, false);
    }
}

async function processPayment() {
    const form = document.getElementById('paymentForm');
    const formData = new FormData(form);
    
    const paymentData = {
        type: formData.get('paymentType'),
        accountNumber: formData.get('paymentAccount'),
        amount: parseFloat(formData.get('paymentAmount')),
        reference: formData.get('paymentReference')
    };
    
    if (!paymentData.type || !paymentData.accountNumber || !paymentData.amount || !paymentData.reference) {
        showToast('Please fill in all fields', 'error');
        return;
    }
    
    if (paymentData.amount <= 0) {
        showToast('Amount must be greater than zero', 'error');
        return;
    }
    
    const submitBtn = form.querySelector('button[type="submit"]') || document.querySelector('#paymentModal .btn-primary');
    setLoading(submitBtn, true);
    
    try {
        const response = await fetch(`${API_BASE}/payment`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(paymentData)
        });
        
        if (response.ok) {
            const result = await response.json();
            showToast('Payment successful!', 'success');
            closeModal('paymentModal');
            updateDashboard();
            form.reset();
        } else {
            const error = await response.json();
            showToast(error.message || 'Payment failed', 'error');
        }
    } catch (error) {
        showToast('Network error. Please try again.', 'error');
    } finally {
        setLoading(submitBtn, false);
    }
}

async function processDeposit() {
    const form = document.getElementById('depositForm');
    const formData = new FormData(form);
    
    const depositData = {
        toAccount: formData.get('depositTo'),
        amount: parseFloat(formData.get('depositAmount')),
        method: formData.get('depositMethod')
    };
    
    if (!depositData.toAccount || !depositData.amount || !depositData.method) {
        showToast('Please fill in all fields', 'error');
        return;
    }
    
    if (depositData.amount <= 0) {
        showToast('Amount must be greater than zero', 'error');
        return;
    }
    
    const submitBtn = form.querySelector('button[type="submit"]') || document.querySelector('#depositModal .btn-primary');
    setLoading(submitBtn, true);
    
    try {
        const response = await fetch(`${API_BASE}/deposit`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(depositData)
        });
        
        if (response.ok) {
            const result = await response.json();
            showToast('Deposit successful!', 'success');
            closeModal('depositModal');
            updateDashboard();
            form.reset();
        } else {
            const error = await response.json();
            showToast(error.message || 'Deposit failed', 'error');
        }
    } catch (error) {
        showToast('Network error. Please try again.', 'error');
    } finally {
        setLoading(submitBtn, false);
    }
}

async function processWithdrawal() {
    const form = document.getElementById('withdrawForm');
    const formData = new FormData(form);
    
    const withdrawalData = {
        fromAccount: formData.get('withdrawFrom'),
        amount: parseFloat(formData.get('withdrawAmount')),
        method: formData.get('withdrawMethod')
    };
    
    if (!withdrawalData.fromAccount || !withdrawalData.amount || !withdrawalData.method) {
        showToast('Please fill in all fields', 'error');
        return;
    }
    
    if (withdrawalData.amount <= 0) {
        showToast('Amount must be greater than zero', 'error');
        return;
    }
    
    const submitBtn = form.querySelector('button[type="submit"]') || document.querySelector('#withdrawModal .btn-primary');
    setLoading(submitBtn, true);
    
    try {
        const response = await fetch(`${API_BASE}/withdraw`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(withdrawalData)
        });
        
        if (response.ok) {
            const result = await response.json();
            showToast('Withdrawal successful!', 'success');
            closeModal('withdrawModal');
            updateDashboard();
            form.reset();
        } else {
            const error = await response.json();
            showToast(error.message || 'Withdrawal failed', 'error');
        }
    } catch (error) {
        showToast('Network error. Please try again.', 'error');
    } finally {
        setLoading(submitBtn, false);
    }
}

// Session Management
function startSessionTimer() {
    if (sessionTimer) {
        clearInterval(sessionTimer);
    }
    
    sessionTimer = setInterval(() => {
        const remaining = Math.max(0, sessionTimeout - (Date.now() - sessionStartTime));
        const minutes = Math.floor(remaining / 60000);
        const seconds = Math.floor((remaining % 60000) / 1000);
        
        const countdownElement = document.getElementById('sessionCountdown');
        if (countdownElement) {
            countdownElement.textContent = `${minutes}:${seconds.toString().padStart(2, '0')}`;
        }
        
        if (remaining <= 0) {
            logout();
        } else if (remaining <= 5 * 60 * 1000) { // Show warning 5 minutes before expiry
            document.getElementById('sessionTimeout').style.display = 'block';
        }
    }, 1000);
}

function logout() {
    currentUser = null;
    currentAccount = null;
    
    if (sessionTimer) {
        clearInterval(sessionTimer);
        sessionTimer = null;
    }
    
    // Hide dashboard and show auth
    document.getElementById('dashboard').classList.remove('active');
    document.getElementById('authSection').style.display = 'block';
    document.getElementById('userInfo').style.display = 'none';
    
    // Reset forms
    document.getElementById('loginForm').reset();
    document.getElementById('registerForm').reset();
    
    showToast('Logged out successfully', 'info');
}

// Dashboard Functions
function showDashboard() {
    document.getElementById('authSection').style.display = 'none';
    document.getElementById('dashboard').classList.add('active');
    document.getElementById('userInfo').style.display = 'flex';
    
    if (currentUser) {
        document.getElementById('userName').textContent = currentUser.name;
        document.getElementById('dashboardUserName').textContent = currentUser.name;
    }
    
    updateDashboard();
    updateCurrentTime();
}

function updateDashboard() {
    if (currentUser) {
        // Update balance
        const balanceElement = document.getElementById('accountBalance');
        if (balanceElement) {
            balanceElement.textContent = formatCurrency(currentUser.balance || 0);
        }
        
        // Update transactions
        updateTransactions();
    }
}

function updateTransactions() {
    const transactionsList = document.getElementById('transactionsList');
    if (!transactionsList || !currentUser) return;
    
    const transactions = currentUser.transactions || [];
    
    if (transactions.length === 0) {
        transactionsList.innerHTML = `
            <div style="text-align: center; padding: 40px; color: var(--medium-gray);">
                <i class="fas fa-inbox" style="font-size: 3rem; margin-bottom: 15px; opacity: 0.5;"></i>
                <p>No transactions yet</p>
                <small>Your transaction history will appear here</small>
            </div>
        `;
        return;
    }
    
    const recentTransactions = transactions.slice(0, 5);
    transactionsList.innerHTML = recentTransactions.map(transaction => `
        <div class="transaction-item">
            <div class="transaction-info">
                <div class="transaction-icon ${transaction.type === 'debit' ? 'debit' : 'credit'}">
                    <i class="fas fa-${transaction.type === 'debit' ? 'arrow-down' : 'arrow-up'}"></i>
                </div>
                <div class="transaction-details">
                    <h4>${transaction.description}</h4>
                    <div class="transaction-meta">${transaction.reference || 'No reference'}</div>
                </div>
            </div>
            <div class="transaction-amount">
                <div class="amount ${transaction.type === 'debit' ? 'debit' : 'credit'}">
                    ${transaction.type === 'debit' ? '-' : '+'}${formatCurrency(transaction.amount)}
                </div>
                <div class="transaction-date">${formatDate(transaction.timestamp)}</div>
            </div>
        </div>
    `).join('');
}

function updateCurrentTime() {
    const timeElement = document.getElementById('currentTime');
    if (timeElement) {
        const now = new Date();
        timeElement.textContent = now.toLocaleDateString('en-ZA', {
            weekday: 'long',
            year: 'numeric',
            month: 'long',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit'
        });
    }
}

// Password Strength Checker
function checkPasswordStrength(password) {
    let strength = 0;
    let feedback = [];
    
    if (password.length >= 8) strength++;
    else feedback.push('At least 8 characters');
    
    if (/[a-z]/.test(password)) strength++;
    if (/[A-Z]/.test(password)) strength++;
    if (/[0-9]/.test(password)) strength++;
    if (/[^A-Za-z0-9]/.test(password)) strength++;
    
    const strengthBar = document.getElementById('strengthBar');
    const strengthText = document.getElementById('strengthText');
    
    if (strengthBar && strengthText) {
        strengthBar.className = 'strength-bar';
        
        if (strength <= 2) {
            strengthBar.classList.add('strength-weak');
            strengthText.textContent = 'Weak password';
        } else if (strength <= 3) {
            strengthBar.classList.add('strength-medium');
            strengthText.textContent = 'Medium strength';
        } else {
            strengthBar.classList.add('strength-strong');
            strengthText.textContent = 'Strong password';
        }
    }
}

// Event Listeners
document.addEventListener('DOMContentLoaded', function() {
    console.log('DOM loaded, initializing banking app...');
    
    // Debug: Check if elements exist
    const authSection = document.getElementById('authSection');
    const dashboard = document.getElementById('dashboard');
    console.log('Auth section found:', authSection);
    console.log('Dashboard found:', dashboard);
    
    if (authSection) {
        console.log('Auth section display style:', authSection.style.display);
        console.log('Auth section computed display:', window.getComputedStyle(authSection).display);
    }
    // Form submissions
    const loginForm = document.getElementById('loginForm');
    const registerForm = document.getElementById('registerForm');
    
    if (loginForm) {
        loginForm.addEventListener('submit', login);
    }
    
    if (registerForm) {
        registerForm.addEventListener('submit', register);
    }
    
    // Password strength checker
    const regPassword = document.getElementById('regPassword');
    if (regPassword) {
        regPassword.addEventListener('input', (e) => {
            checkPasswordStrength(e.target.value);
        });
    }
    
    // Add ripple effects to buttons
    document.querySelectorAll('.btn-primary, .btn-secondary, .btn-account-action').forEach(button => {
        button.addEventListener('click', addRippleEffect);
    });
    
    // Update time every minute
    setInterval(updateCurrentTime, 60000);
    
    // Check if user is already logged in
    checkAuthStatus();
    
    // Ensure auth section is visible by default
    if (authSection) {
        authSection.style.display = 'block';
        console.log('Auth section display set to block');
        console.log('Auth section computed style:', window.getComputedStyle(authSection).display);
    } else {
        console.error('Auth section not found!');
    }
});

// Check authentication status on page load
async function checkAuthStatus() {
    try {
        const response = await fetch(`${API_BASE}/status`);
        if (response.ok) {
            const userData = await response.json();
            currentUser = userData;
            showDashboard();
            startSessionTimer();
        } else {
            // Show authentication section by default
            showAuthSection();
        }
    } catch (error) {
        // User not authenticated, show login by default
        console.log('User not authenticated, showing login form');
        showAuthSection();
    }
}

// Show authentication section
function showAuthSection() {
    document.getElementById('authSection').style.display = 'block';
    document.getElementById('dashboard').classList.remove('active');
    document.getElementById('userInfo').style.display = 'none';
}

// Add CSS for ripple effect
const style = document.createElement('style');
style.textContent = `
    .ripple {
        position: absolute;
        border-radius: 50%;
        background: rgba(255, 255, 255, 0.6);
        transform: scale(0);
        animation: ripple-animation 0.6s linear;
        pointer-events: none;
    }
    
    @keyframes ripple-animation {
        to {
            transform: scale(4);
            opacity: 0;
        }
    }
    
    .focused {
        transform: scale(1.02);
    }
    
    .has-value {
        border-color: var(--primary-color) !important;
    }
    
    .transaction-item {
        opacity: 0;
        animation: slideInRight 0.5s ease forwards;
    }
    
    .modal {
        opacity: 0;
        transition: opacity 0.3s ease;
    }
    
    .modal.show {
        opacity: 1;
    }
    
    .auth-section,
    .dashboard-section {
        opacity: 0;
        transform: translateY(0);
        transition: all 0.3s ease;
    }
`;
document.head.appendChild(style); 