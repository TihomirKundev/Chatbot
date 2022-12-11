import LetteredAvatar from 'react-lettered-avatar';

export const ChatMessage = ({senderName, message} : {senderName: string, message: string}) =>
    senderName ?
        <div className="message">
            <div className="photo">
                <LetteredAvatar name={senderName}/>
            </div>
            <p className="text">{message}</p>
        </div> :
        <div className="message text-only">
            <div className="response">
                <p className="text">{message}</p>
            </div>
        </div>;

export default ChatMessage;
