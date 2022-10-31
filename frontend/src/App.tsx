import './App.css';
import  './components/TechSupport/styles/style.css';
import  './components/TechSupport/styles/css_bootstrap.min.css';
import TechSupportPage from "./components/TechSupport/TechSupportPage";
import ChatBot from "./components/Customer/ChatBot";
import LoginPage from "./components/Login/LoginPage";
import RegisterPage from "./components/Register/RegisterPage";
import {BrowserRouter as Router, Route, Routes as Switch} from "react-router-dom";

function App() {
    return (
        <Router>

            <div className="App">
                <Switch>
                    <Route path="/customer"  element={<ChatBot/>}/>
                    <Route path="/admin"  element={<TechSupportPage/>}/>
                    <Route path="/" element={<LoginPage/>}/>
                    <Route path="/register" element={<RegisterPage/>}/>
                </Switch>
            </div>


        </Router>
    );
}

export default App;
