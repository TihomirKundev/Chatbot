import functools

_questions = {
'What can I do when I have issues with my account?':
'Please contact our BAS World service desk: https://www.basworld.com/contact-us',
'Will you send me an update if new vehicles become available that match my Saved Search criteria?':
'Yes, we will! We send BAS World Saved Search email update and show you newly available vehicles.',
'Why should I mark a vehicle as a favourite?':
'You can see all your favourite vehicles quickly and easily in your BAS World dashboard – and you don\'t have to search for these again. You will also get updates about any price reductions and see when a vehicle is sold.',
'What will happen after I placed my order?':
'After the order is placed and the payment is fulfilled the required documents will be sent by the seller. When the required documents are sent, the vehicle can be picked up, or will be delivered to your location.',
'Can I buy a vehicle located in another country?':
'Sure, why not? You need to import a vehicle to your country, but that shouldn\'t be too difficult. However, please keep in mind that there are different rules regarding the import of a vehicle, and that you have verified any applicable customs fees and taxes. We advise you to seek advice from your local registration authority and country\'s customs office at all times.',
'Is it safe to buy a vehicle without seeing the vehicle in real life?':
'Many people buy vehicles without a physical inspection these days. Photos, videos, specifications and detailed information about the condition can give you enough information to make the decision to buy a vehicle – which also saves you time and money.',
'What is "Saved Search"?':
'When you do a detailed search on BAS World, you can save this search so you can perform the very same detailed search at a later point in time with just a single click. Our system can also send you updates to inform you about newly listed vehicles that match the criteria of your Saved Searches – to make sure you\'re always the first to know about newly listed vehicles that may be just what you are looking for.',
'I want to buy a specific vehicle, how do I get in touch with the seller?':
'Just fill in your preferred specifications, choose the desired vehicle and start a chat with the seller on https://www.basworld.com or via our application.',
'How many vehicles can I add to my watch list?':
'As many as you like.',
'Is my information safe on BAS World?':
'It is! We keep all data safe and secure in line with laws and regulations in the EU.',
'How do I change my delivery address after I have ordered a vehicle?':
'Contact your seller via BAS World chat and tell him or her the new delivery address.',
'Where can I find the vessel schedule?':
'In your transport confirmation you can find all details on the scheme.',
'Can I change the delivery address after the transport confirmation?':
'No this is only possible before the transport confirmation.',
'Can I trace my vehicle during transport if BAS World has arranged the delivery?':
'We will give updates during each step in the transport process when you choose for BAS World\'s Delivery Service.',
'Do I have to pay taxes or fees when importing a vehicle?':
'Probably you will have to. The tax and fee structure differs per country as each country has different regulations. Contact your country\'s relevant institution to enquiry about possible taxes and fees.',
'When can I expect the Bill of Lading after my vehicle is shipped?':
'As soon as the vehicle is on its way, the Bill of Lading (BL) will be drafted. At that moment you will be informed and provided with the BL.',
'How do I get the vehicle to my business address?':
'This depends on the location of the vehicle. You can choose to pick up the vehicle directly from the seller, or have the vehicle transported or shipped to your preferred port. By using the Safe Deal service, you have the possibility to let us arrange the delivery of your vehicle. You can find more information about Safe Deal here: https://www.basworld.com/home.',
'How do I get my vehicle out of the port?':
'This differs in every port. For the correct information please contact the port of arrival or the company who arranged shipping.',
'How am I insured on transit plates?':
'Transit plates have third party liability insurance, which means the vehicle is insured for damages caused to others. Please always make sure you check if the coverage is sufficient for you.',
'How am I insured for transport?':
'By choosing Safe Deal you can make use of our delivery service, by using the delivery service provided by us you are insured under the CMR conditions. If you don\'t make use of our delivery service you\'ll have to discuss with your local insurance agent what the ideal insurance option would be.',
'Do you offer a delivery guarantee?':
'If you\'re using Safe Deal, a guarantee will be offered that the vehicle in the pictures will actually be delivered.',
'Do I need to buy a vignette or pay toll when I want to pick up my vehicle over the road?':
'This is different in every country, you have to check this at the border of every country you enter.',
'Can I reach the company that transports my vehicles to me?':
'If you have chosen for the transport option offered by BAS World you can always contact our delivery desk here: https://www.basworld.com/contact-us',
'What do I need to fill in on a CMR?':
'Please see the following example for a complete signed: https://www.basworld.com/files/cms/pdf/cmr_proof_of_delivery.pdf.',
'Can I change my password?':
'Yes, you can do that here: https://www.basworld.com/ashboard/my-settings/preferences.',
'Can I change my information?':
'Yes, you can do that here: https://www.basworld.com/dashboard/my-settings/my-profile.'
}

_prompt = functools.reduce(lambda value, element: value + 'Question: ' + element + '\nAnswer: ' + _questions[element] + '\n', _questions, '') + 'Question: '

def getPromptWithQuestion(question: str):
        return _prompt + question + '\nAnswer:'