import './App.css';
import  './components/TechSupport/styles/style.css';
import  './components/TechSupport/styles/css_bootstrap.min.css';
import TechSupportPage from "./components/TechSupport/TechSupportPage";
import ChatBot from "./components/Customer/ChatBot";


function App() {
  return (
    <div className="App">
        <ChatBot />
        <TechSupportPage />
    </div>
  );
}

export default App;
