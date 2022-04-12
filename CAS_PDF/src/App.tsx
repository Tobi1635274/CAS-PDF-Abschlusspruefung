import './App.css';
import '@progress/kendo-theme-default/dist/all.css';
import NavBar from './components/NavBar';
import Pages from './Pages';
import { BrowserRouter } from 'react-router-dom';

function App() {
    return (
        <div className="App">
            <BrowserRouter>
                <NavBar />
                <Pages />
            </ BrowserRouter>
        </div>
    );
}

export default App;
