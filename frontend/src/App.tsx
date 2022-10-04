import './App.css';
import  './components/TechSupport/styles/style.css';
import  './components/TechSupport/styles/css_bootstrap.min.css';
import TechSupportPage from "./components/TechSupport/TechSupportPage";
import ChatBot from "./components/Customer/ChatBot";
import {BrowserRouter as Router, Route, Routes as Switch} from "react-router-dom";

function App() {
  return (
      <Router>
          
        <div className="App">
            <Switch>
                <Route exact path="/"  element={<ChatBot/>}/>
                <Route exact path="/admin"  element={<TechSupportPage/>}/>
            </Switch>
        </div>
        
        
      </Router>
  );
}

export default App;
