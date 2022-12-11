from DTO.OrderQuestionRequestDTO import orderQuestionDTO


def OrderWithPrompt(incOrderQuestion: orderQuestionDTO):
    Prompt = f'''
Customer:
customerId:{incOrderQuestion.user.id}
firstName:{incOrderQuestion.user.firstName}
lastName:{incOrderQuestion.user.lastName}
email:{incOrderQuestion.user.email}
phone:{incOrderQuestion.user.phone}
    '''
    for o in incOrderQuestion.user.orders:
        Prompt += f'''
Order:
orderNumber: 12345678765 #add order number
orderStatus:{o.status.name}
        '''
        for v in o.vehicles:  # TODO:maybe change Vehicle to truck
            Prompt += f'''
Vehicle:
VehicleReferenceNum:{v.referenceNum}
Name:{v.name}
availability:{v.availability}
location:{v.location}
price:{v.price}
city:{v.city}
            '''
    Prompt += f'''
Question:{incOrderQuestion.question}
Answer:
    '''
    return Prompt
