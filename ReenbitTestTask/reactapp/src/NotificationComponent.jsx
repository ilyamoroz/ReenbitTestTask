import React from 'react';
import './NotificationComponent.css'

function NotificationComponent({ message }) {
    return (
        <div className="notification">
            {message}
        </div>
    );
}

export default NotificationComponent;