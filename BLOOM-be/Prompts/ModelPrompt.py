_promptText = """This list shows how to divide questions into FAQ and Order. All general questions go to FAQ. Only specific questions are for Order.
***
Question: What is BAS world?
Type: FAQ
***
Question: When will my truck arrive?
Type: Order
***
Question: When is BAS world open?
Type: FAQ
***
Question: Where is my vehicle?
Type: Order
***
Question: What kinds of vehicles can I order?
Type: FAQ
***
Question: How do I order a tractor?
Type: FAQ
***
Question: Why isn't my car here yet?
Type: Order
***
Question: What is your phone number?
Type: FAQ
***
Question: When are you open?
Type: FAQ
***
Question: how do i change my password
Type: FAQ
***
Question: can i change my password
Type: FAQ
***
Question: """

def getPromptWithQuestion(question: str):
    return _promptText + question + "\nType:"
