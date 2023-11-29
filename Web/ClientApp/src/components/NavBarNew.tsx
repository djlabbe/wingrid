import { Avatar, Dropdown, Navbar } from 'flowbite-react';
import { useLoginContext } from '../hooks/useLoginContext';
import { useNavigate } from 'react-router-dom';

const NavBarNew = () => {
  const { loginResult, logout } = useLoginContext();
  const navigate = useNavigate();

  const handleSignOut = async () => {
    if (logout) logout();
    navigate("/");
  }

  const user = loginResult?.user;
  const isAdmin = loginResult?.roles?.includes("ADMIN");

  return (
    <Navbar fluid rounded className="items-start">
      <Navbar.Brand href="/">
        <img src="/logo.svg" className="mr-3 h-6 sm:h-9" alt="Wingrid Logo" />
        <span className="self-center whitespace-nowrap text-xl font-semibold dark:text-white">WINGRID</span>
      </Navbar.Brand>
      <div className="sm:order-last">
        {user && <Dropdown
          arrowIcon={false}
          inline
          label={
            <Avatar alt="User settings" img="/avatar.svg" rounded />
          }
        >
          <Dropdown.Header>
            <span className="block text-sm">{user.name}</span>
            <span className="block truncate text-sm font-medium">{user.email}</span>
          </Dropdown.Header>
          <Dropdown.Item>Profile</Dropdown.Item>
          <Dropdown.Item>Settings</Dropdown.Item>
          <Dropdown.Divider />
          <Dropdown.Item onClick={handleSignOut}>Sign out</Dropdown.Item>
        </Dropdown>}
        <Navbar.Toggle />
      </div>
      {user && <Navbar.Collapse>
        <Navbar.Link href="/dashboard">Dashboard</Navbar.Link>
        {isAdmin && <Navbar.Link href="/admin">Admin</Navbar.Link>}
        <Navbar.Link href="/about">About</Navbar.Link>
      </Navbar.Collapse>
      }
    </Navbar>
  );
}

export default NavBarNew;