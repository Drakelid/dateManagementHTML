// DOM Elements
document.addEventListener('DOMContentLoaded', () => {
    // Navigation functionality
    const navItems = document.querySelectorAll('.nav-item');
    const createEventBtn = document.querySelector('.create-event-btn');
    const refreshBtn = document.querySelector('.refresh-btn');
    const notificationBtn = document.querySelector('.notification-btn');
    
    // Current date display function
    updateDate();
    
    // Add event listeners to navigation items
    navItems.forEach(item => {
        item.addEventListener('click', (e) => {
            // The link navigation is handled by the browser naturally
            // We don't need to add manual navigation here anymore
        });
    });
    
    // Create event button click
    if (createEventBtn) {
        createEventBtn.addEventListener('click', () => {
            window.location.href = 'events.html';
        });
    }
    
    // Refresh button functionality
    if (refreshBtn) {
        refreshBtn.addEventListener('click', () => {
            window.location.reload();
        });
    }
    
    // Notification button functionality
    if (notificationBtn) {
        notificationBtn.addEventListener('click', () => {
            alert('Notifications will be displayed here!');
        });
    }
    
    // Reminders page functionality
    const createReminderBtn = document.querySelector('.create-reminder-btn');
    if (createReminderBtn) {
        createReminderBtn.addEventListener('click', () => {
            alert('Create Reminder functionality will be implemented here!');
        });
    }
    
    // Settings save button functionality
    const saveSettingsBtn = document.querySelector('.save-settings-btn');
    if (saveSettingsBtn) {
        saveSettingsBtn.addEventListener('click', () => {
            alert('All settings have been saved successfully!');
        });
    }
    
    // Filter buttons functionality
    const filterBtns = document.querySelectorAll('.filter-btn');
    if (filterBtns.length > 0) {
        filterBtns.forEach(btn => {
            btn.addEventListener('click', () => {
                // Remove active class from all filter buttons
                filterBtns.forEach(b => b.classList.remove('active'));
                
                // Add active class to clicked button
                btn.classList.add('active');
            });
        });
    }
    
    // Category buttons functionality
    const categoryBtns = document.querySelectorAll('.category-btn');
    if (categoryBtns.length > 0) {
        categoryBtns.forEach(btn => {
            btn.addEventListener('click', () => {
                // Remove active class from all category buttons
                categoryBtns.forEach(b => b.classList.remove('active'));
                
                // Add active class to clicked button
                btn.classList.add('active');
            });
        });
    }
    
    // Settings page tabs functionality
    const settingsNavItems = document.querySelectorAll('.settings-nav-item');
    const settingsTabs = document.querySelectorAll('.settings-tab');
    
    if (settingsNavItems.length > 0 && settingsTabs.length > 0) {
        settingsNavItems.forEach(item => {
            item.addEventListener('click', () => {
                // Get the tab to show
                const tabToShow = item.dataset.tab;
                
                // Remove active class from all nav items and tabs
                settingsNavItems.forEach(navItem => navItem.classList.remove('active'));
                settingsTabs.forEach(tab => tab.classList.remove('active'));
                
                // Add active class to clicked nav item
                item.classList.add('active');
                
                // Show the corresponding tab
                document.getElementById(tabToShow).classList.add('active');
            });
        });
    }
    
    // Calendar functionality
    const calendarNavBtns = document.querySelectorAll('.calendar-nav-btn');
    if (calendarNavBtns.length > 0) {
        calendarNavBtns.forEach(btn => {
            btn.addEventListener('click', () => {
                // This would typically change the month/year display
                // For demo purposes, we'll just show an alert
                alert('Calendar navigation will be implemented here!');
            });
        });
    }
    
    // Calendar day selection
    const calendarDays = document.querySelectorAll('.calendar-day');
    if (calendarDays.length > 0) {
        calendarDays.forEach(day => {
            day.addEventListener('click', () => {
                // Remove selected class from all days
                calendarDays.forEach(d => d.classList.remove('selected'));
                
                // Add selected class to clicked day
                day.classList.add('selected');
                
                // For a real implementation, this would update the events display
                if (!day.classList.contains('inactive')) {
                    // Update events for this day (demo only)
                    const dayNumber = day.textContent;
                    const eventsTitle = document.querySelector('.calendar-events h3');
                    if (eventsTitle) {
                        eventsTitle.textContent = `Events for April ${dayNumber}`;
                    }
                }
            });
        });
    }
});

// Current date display function
function updateDate() {
    const today = new Date();
    const options = { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' };
    
    // You can add a date display element to the HTML and update it here if needed
    console.log(today.toLocaleDateString('en-US', options));
}
