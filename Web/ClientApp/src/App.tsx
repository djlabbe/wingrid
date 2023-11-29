import { BrowserRouter } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { ToastContainer } from 'react-toastify';
import { LoginProvider } from './hooks/LoginContext';
import NavBar from './components/NavBar';


const App = () => {
  return (
    <LoginProvider>
      <BrowserRouter>
        <ToastContainer />
        <NavBar />
        <AppRoutes/>
      </BrowserRouter>
    </LoginProvider>
  )
};

export default App;
