import logo from './Images/poke.png';
import background from './Images/backgrounds.jpg';
import './App.css';
import SearchComponent   from './SearchComponent';

function App() {
  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <br></br>
        <SearchComponent/>
      </header>
    </div>
  );
}

export default App;
