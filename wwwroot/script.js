// Global variables
let currentUser = null;

// Initialize app
document.addEventListener('DOMContentLoaded', function() {
    updateTime();
    setInterval(updateTime, 1000);
    initializePasswordStrength();
    setupHeaderScroll();
    setupAccessibility();
    setupEventListeners();
});

// Setup event listeners
function setupEventListeners() {
    // Tab buttons
    document.getElementById('loginTabBtn').addEventListener('click', () => switchTab('login'));
    document.getElementById('registerTabBtn').addEventListener('click', () => switchTab('register'));
    
    // Form submissions
    document.getElementById('loginForm').addEventListener('submit', handleLogin);
    document.getElementById('registerForm').addEventListener('submit', handleRegister);
    
    // Logout button
    document.getElementById('logoutBtn').addEventListener('click', logout);
}

// Header scroll effect
function setupHeaderScroll() {
    const header = document.getElementById('header');
    let ticking = false;
    
    function updateHeader() {
        if (window.scrollY > 50) {
            header.classList.add('scrolled');
        } else {
            header.classList.remove('scrolled');
        }
        ticking = false;
    }
    
    window.addEventListener('scroll', () => {
        if (!ticking) {
            requestAnimationFrame(updateHeader);
            ticking = true;
        }
    });
}

// Enhanced Accessibility
function setupAccessibility() {
    // Focus management for modals and tabs
    document.addEventListener('keydown', function(e) {
        // ESC key to close modals or focus management
        if (e.key === 'Escape') {
            const activeModal = document.querySelector('.modal[style*="block"]');
            if (activeModal) {
                activeModal.style.display = 'none';
                document.body.style.overflow = 'auto';
            }
        }
        
        // Tab navigation improvements
        if (e.key === 'Tab') {
            document.body.classList.add('keyboard-navigation');
        }
    });
    
    // Remove keyboard navigation class on mouse use
    document.addEventListener('mousedown', function() {
        document.body.classList.remove('keyboard-navigation');
    });
    
    // Announce dynamic content changes for screen readers
    const announcer = document.createElement('div');
    announcer.setAttribute('aria-live', 'polite');
    announcer.setAttribute('aria-atomic', 'true');
    announcer.className = 'sr-only';
    announcer.style.cssText = 'position: absolute; left: -10000px; width: 1px; height: 1px; overflow: hidden;';
    document.body.appendChild(announcer);
    
    window.announce = function(message) {
        announcer.textContent = '';
        setTimeout(() => {
            announcer.textContent = message;
        }, 100);
    };
}

// Authentication Functions
function switchTab(tab) {
    const tabs = document.querySelectorAll('.tab-btn');
    const contents = document.querySelectorAll('.tab-content');
    
    tabs.forEach(t => t.classList.remove('active'));
    contents.forEach(c => c.classList.remove('active'));
    
    const activeTabBtn = document.querySelector(`#${tab}TabBtn`);
    const activeTabContent = document.getElementById(tab + 'Tab');
    
    if (activeTabBtn && activeTabContent) {
        activeTabBtn.classList.add('active');
        activeTabContent.classList.add('active');
        
        // Focus first input in the active tab
        const firstInput = activeTabContent.querySelector('input');
        if (firstInput) {
            setTimeout(() => firstInput.focus(), 100);
        }
        
        // Announce tab change
        if (window.announce) {
            window.announce(`${tab === 'login' ? 'Sign in' : 'Create account'} form selected`);
        }
    }
}

function initializePasswordStrength() {
    const passwordInput = document.getElementById('regPassword');
    if (passwordInput) {
        passwordInput.addEventListener('input', function() {
            checkPasswordStrength(this.value);
        });
    }
}

function checkPasswordStrength(password) {
    const strengthBar = document.getElementById('strengthBar');
    const strengthText = document.getElementById('strengthText');
    
    if (!strengthBar || !strengthText) return;
    
    let strength = 0;
    let feedback = [];

    if (password.length >= 8) strength += 25;
    else feedback.push('At least 8 characters');

    if (/[A-Z]/.test(password)) strength += 25;
    else feedback.push('One uppercase letter');

    if (/[a-z]/.test(password)) strength += 25;
    else feedback.push('One lowercase letter');

    if (/[0-9]/.test(password)) strength += 25;
    else feedback.push('One number');

    strengthBar.className = 'strength-bar';
    
    if (strength < 50) {
        strengthBar.classList.add('strength-weak');
        strengthText.textContent = 'Weak - ' + feedback.join(', ');
        strengthText.style.color = 'var(--error)';
    } else if (strength < 100) {
        strengthBar.classList.add('strength-medium');
        strengthText.textContent = 'Good - ' + feedback.join(', ');
        strengthText.style.color = 'var(--warning)';
    } else {
        strengthBar.classList.add('strength-strong');
        strengthText.textContent = 'Strong password';
        strengthText.style.color = 'var(--success)';
    }
}

// Form Handlers
function handleLogin(e) {
    e.preventDefault();
    showLoading();
    
    setTimeout(() => {
        hideLoading();
        showDashboard('Amahle Dlamini');
        showNotification('Login successful! Welcome back.', 'success');
        if (window.announce) {
            window.announce('Successfully logged in. Dashboard loaded.');
        }
    }, 2000);
}

function handleRegister(e) {
    e.preventDefault();
    const password = document.getElementById('regPassword').value;
    const confirmPassword = document.getElementById('regConfirmPassword').value;
    
    if (password !== confirmPassword) {
        showNotification('Passwords do not match!', 'error');
        return;
    }
    
    showLoading();
    setTimeout(() => {
        hideLoading();
        showNotification('Account created successfully! Please verify your email.', 'success');
        switchTab('login');
        if (window.announce) {
            window.announce('Account created successfully. Please sign in.');
        }
    }, 2500);
}

// Dashboard Functions
function showDashboard(userName) {
    document.getElementById('authSection').style.display = 'none';
    document.getElementById('dashboard').classList.add('active');
    document.getElementById('userInfo').style.display = 'flex';
    document.getElementById('userName').textContent = userName;
    currentUser = userName;
    
    // Update page title
    document.title = `NexusBank - Dashboard - ${userName}`;
}

function logout() {
    if (confirm('Are you sure you want to logout?')) {
        showLoading();
        setTimeout(() => {
            hideLoading();
            document.getElementById('authSection').style.display = 'block';
            document.getElementById('dashboard').classList.remove('active');
            document.getElementById('userInfo').style.display = 'none';
            currentUser = null;
            showNotification('Logged out successfully.', 'info');
            document.title = 'NexusBank - Next Generation Banking';
            
            // Focus on login form
            const loginId = document.getElementById('loginId');
            if (loginId) {
                setTimeout(() => loginId.focus(), 500);
            }
            
            if (window.announce) {
                window.announce('Logged out successfully. Please sign in again.');
            }
        }, 1500);
    }
}

function updateTime() {
    const now = new Date();
    const options = { 
        weekday: 'long', 
        year: 'numeric', 
        month: 'long', 
        day: 'numeric',
        hour: '2-digit',
        minute: '2-digit'
    };
    const timeElement = document.getElementById('currentTime');
    if (timeElement) {
        timeElement.textContent = now.toLocaleDateString('en-ZA', options);
    }
}

// Utility Functions
function showLoading() {
    const overlay = document.getElementById('loadingOverlay');
    overlay.style.display = 'flex';
    overlay.setAttribute('aria-hidden', 'false');
    document.body.style.overflow = 'hidden';
}

function hideLoading() {
    const overlay = document.getElementById('loadingOverlay');
    overlay.style.display = 'none';
    overlay.setAttribute('aria-hidden', 'true');
    document.body.style.overflow = 'auto';
}

function showNotification(message, type) {
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `notification ${type}`;
    notification.innerHTML = `
        <i class="fas ${type === 'success' ? 'fa-check-circle' : type === 'error' ? 'fa-exclamation-circle' : 'fa-info-circle'}"></i>
        <span>${message}</span>
    `;
    
    document.body.appendChild(notification);
    
    // Show notification
    setTimeout(() => {
        notification.classList.add('show');
    }, 100);
    
    // Remove notification after delay
    setTimeout(() => {
        notification.classList.remove('show');
        setTimeout(() => {
            if (notification.parentNode) {
                notification.parentNode.removeChild(notification);
            }
        }, 300);
    }, 5000);
}