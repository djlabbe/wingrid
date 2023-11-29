import NavBar from './components/NavBar';
import { BrowserRouter } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import { ToastContainer } from 'react-toastify';
import { LoginProvider } from './hooks/LoginContext';
import NavBarNew from './components/NavBarNew';


const App = () => {
  return (
    <LoginProvider>
      <BrowserRouter>
        <ToastContainer />
        <NavBarNew />
        <AppRoutes/>
      </BrowserRouter>
    </LoginProvider>
  )
};

export default App;
