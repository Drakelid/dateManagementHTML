document.addEventListener('DOMContentLoaded', () => {
    // Login form handling
    const loginForm = document.getElementById('loginForm');
    if (loginForm) {
        loginForm.addEventListener('submit', (e) => {
            e.preventDefault();
            
            const email = document.getElementById('email').value;
            const password = document.getElementById('password').value;
            const rememberMe = document.getElementById('rememberMe')?.checked || false;
            
            // Basic validation
            if (!email || !password) {
                showMessage('Please fill in all fields', 'error');
                return;
            }
            
            if (!isValidEmail(email)) {
                showMessage('Please enter a valid email address', 'error');
                return;
            }
            
            // Mock login - in a real app, this would call an API
            console.log('Login attempt:', { email, password, rememberMe });
            
            // Simulate successful login
            showMessage('Login successful! Redirecting...', 'success');
            
            // Store in localStorage if remember me is checked
            if (rememberMe) {
                localStorage.setItem('userEmail', email);
            }
            
            // Redirect to dashboard after a short delay
            setTimeout(() => {
                window.location.href = 'index.html';
            }, 1500);
        });
    }
    
    // Registration form handling
    const registerForm = document.getElementById('registerForm');
    if (registerForm) {
        registerForm.addEventListener('submit', (e) => {
            e.preventDefault();
            
            const fullName = document.getElementById('fullName').value;
            const email = document.getElementById('email').value;
            const password = document.getElementById('password').value;
            const confirmPassword = document.getElementById('confirmPassword').value;
            const agreeTerms = document.getElementById('agreeTerms')?.checked || false;
            
            // Basic validation
            if (!fullName || !email || !password || !confirmPassword) {
                showMessage('Please fill in all fields', 'error');
                return;
            }
            
            if (!isValidEmail(email)) {
                showMessage('Please enter a valid email address', 'error');
                return;
            }
            
            if (password !== confirmPassword) {
                showMessage('Passwords do not match', 'error');
                return;
            }
            
            if (password.length < 8) {
                showMessage('Password must be at least 8 characters long', 'error');
                return;
            }
            
            if (!agreeTerms) {
                showMessage('You must agree to the Terms of Service', 'error');
                return;
            }
            
            // Mock registration - in a real app, this would call an API
            console.log('Registration attempt:', { fullName, email, password });
            
            // Simulate successful registration
            showMessage('Account created successfully! Redirecting to login...', 'success');
            
            // Redirect to login page after a short delay
            setTimeout(() => {
                window.location.href = 'login.html';
            }, 1500);
        });
    }
    
    // Check if user email is stored in localStorage (for "Remember me" feature)
    const storedEmail = localStorage.getItem('userEmail');
    if (storedEmail && loginForm) {
        const emailInput = document.getElementById('email');
        if (emailInput) {
            emailInput.value = storedEmail;
            document.getElementById('rememberMe').checked = true;
        }
    }
    
    // Helper function to validate email format
    function isValidEmail(email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }
    
    // Helper function to show messages
    function showMessage(message, type = 'info') {
        // Check if message container exists, if not create it
        let messageContainer = document.querySelector('.message-container');
        
        if (!messageContainer) {
            messageContainer = document.createElement('div');
            messageContainer.className = 'message-container';
            document.body.appendChild(messageContainer);
        }
        
        // Create message element
        const messageElement = document.createElement('div');
        messageElement.className = `message ${type}`;
        messageElement.textContent = message;
        
        // Add to container
        messageContainer.appendChild(messageElement);
        
        // Remove after 3 seconds
        setTimeout(() => {
            messageElement.classList.add('fade-out');
            setTimeout(() => {
                messageElement.remove();
                
                // Remove container if empty
                if (messageContainer.children.length === 0) {
                    messageContainer.remove();
                }
            }, 500);
        }, 3000);
    }
    
    // Forgot password link
    const forgotPasswordLink = document.querySelector('.forgot-password');
    if (forgotPasswordLink) {
        forgotPasswordLink.addEventListener('click', (e) => {
            e.preventDefault();
            showMessage('Password reset functionality will be implemented soon!', 'info');
        });
    }
});
