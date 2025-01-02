import { BrowserRouter } from "react-router-dom";
import NavBar from './components/NavBar.jsx';
import AppRoutes from './routes/AppRoutes.jsx';
import './App.css';

function App() {
  return (
    <>
      <BrowserRouter>
        <NavBar />
        <div className=" my-5">
          <AppRoutes />
        </div>
      </BrowserRouter>
    </>
  );
}

export default App;