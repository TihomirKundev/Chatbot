from DTO.OrderQuestionRequestDTO import orderQuestionDTO

import json

def OrderWithPrompt(incOrderQuestion: orderQuestionDTO):
    user = incOrderQuestion.user.to_json()
    print(user)
    first_order = user['Orders'][0]
    first_vehicle = first_order['Vehicles'][0]
    user_json = json.dumps(user, indent='\t', ensure_ascii=False)

    return f"""This is an application that can retrieve data from a JSON file.
***
Data: {user_json}
***
Question: Where is vehicle {first_vehicle['Vehicle']}?
Answer: The order's status is {first_order['Status']}. The vehicle is now in {first_vehicle['Location']}.
Question: {incOrderQuestion.question}
Answer:"""
