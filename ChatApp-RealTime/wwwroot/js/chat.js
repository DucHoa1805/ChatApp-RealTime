"use strict";

const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .withAutomaticReconnect()
    .build();

// Receive a message
connection.on("ReceiveMessage", function (msg) {
    const isFromPartner =
        msg.senderUsername === chatPartner || msg.receiverUsername === chatPartner;
    const isFromMe = msg.senderUsername === currentUsername;

    // Only display if it belongs to the current conversation
    if (chatPartner && (isFromMe || isFromPartner)) {
        appendMessage(msg.senderUsername, msg.content, msg.timestamp, isFromMe);
    }
});

// Online/offline status updates
connection.on("UserConnected", function (username) {
    updateUserStatus(username, true);
});

connection.on("UserDisconnected", function (username) {
    updateUserStatus(username, false);
});

connection.on("OnlineUsers", function (users) {
    users.forEach(function (u) {
        updateUserStatus(u, true);
    });
});

// Start the connection
connection.start()
    .then(function () {
        console.log("SignalR connected.");
        const sendBtn = document.getElementById("sendBtn");
        if (sendBtn) sendBtn.disabled = false;
    })
    .catch(function (err) {
        console.error("SignalR connection error:", err);
    });

// Send a message
function sendMessage() {
    const input = document.getElementById("messageInput");
    if (!input) return;

    const content = input.value.trim();
    if (!content || !chatPartner) return;

    connection.invoke("SendMessage", chatPartner, content)
        .catch(function (err) {
            console.error("SendMessage error:", err);
        });

    input.value = "";
    input.focus();
}

// Press Enter to send
function handleEnter(event) {
    if (event.key === "Enter") {
        event.preventDefault();
        sendMessage();
    }
}

// Append a message bubble to the chat container
function appendMessage(senderUsername, content, timestamp, isMine) {
    const container = document.getElementById("messageContainer");
    if (!container) return;

    const wrapper = document.createElement("div");
    wrapper.className = "message-wrapper " + (isMine ? "mine" : "theirs");

    let avatarHtml = "";
    if (!isMine) {
        avatarHtml = `<div class="avatar avatar-xs">${senderUsername[0].toUpperCase()}</div>`;
    }

    const timeStr = new Date(timestamp).toLocaleTimeString("vi-VN", {
        hour: "2-digit",
        minute: "2-digit"
    });

    wrapper.innerHTML = `
        ${avatarHtml}
        <div class="message-bubble ${isMine ? "bubble-mine" : "bubble-theirs"}">
            <div class="message-text">${escapeHtml(content)}</div>
            <div class="message-time">${timeStr}</div>
        </div>
    `;

    container.appendChild(wrapper);
    scrollToBottom();
}

// Navigate to a conversation with a selected user
function selectUser(username) {
    window.location.href = "/Chat?partner=" + encodeURIComponent(username);
}

// Update a user's online indicator
function updateUserStatus(username, isOnline) {
    const statusEl = document.getElementById("status-" + username);
    if (statusEl) {
        statusEl.textContent = isOnline ? "● Online" : "● Offline";
        statusEl.style.color = isOnline ? "#22c55e" : "#9ca3af";
    }
    if (username === chatPartner) {
        const partnerStatus = document.getElementById("partnerStatus");
        if (partnerStatus) {
            partnerStatus.textContent = isOnline ? "● Online" : "● Offline";
            partnerStatus.style.color = isOnline ? "#22c55e" : "#9ca3af";
        }
    }
}

// Scroll to the bottom of the message container
function scrollToBottom() {
    const container = document.getElementById("messageContainer");
    if (container) {
        container.scrollTop = container.scrollHeight;
    }
}

// Client-side HTML escape to prevent XSS
function escapeHtml(text) {
    const map = { "&": "&amp;", "<": "&lt;", ">": "&gt;", '"': "&quot;", "'": "&#039;" };
    return text.replace(/[&<>"']/g, function (m) { return map[m]; });
}

// Filter users in sidebar search
document.addEventListener("DOMContentLoaded", function () {
    scrollToBottom();

    const searchInput = document.getElementById("searchUsers");
    if (searchInput) {
        searchInput.addEventListener("input", function () {
            const query = this.value.toLowerCase();
            document.querySelectorAll(".user-item").forEach(function (item) {
                const name = item.getAttribute("data-username") || "";
                item.style.display = name.toLowerCase().includes(query) ? "" : "none";
            });
        });
    }
});
