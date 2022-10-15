1. User send GET request
2. Server check if user has cookie
    1. If user is not logged in AND does not have an ID cookie, create one
    2. Else, use the existing cookie ID
3. User sends a message through the input box with their ID
```json
{
    "id": "b40502d0-3072-4e2a-b2c8-27dc5f57bcaa",
    "message": "Hello!"
}
```
4. Server determines if the ID corresponds to a user
4. Server creates a conversation and adds the client
5. Server responds to user `"Please wait while we connect you to our customer service agent..."`
6. Agent client gets updated with a new conversation and selects the new option
7. Server adds the agent as a User object to the conversation
8. Communication starts