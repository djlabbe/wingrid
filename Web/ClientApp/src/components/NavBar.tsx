import { Fragment } from 'react'
import { Disclosure, Menu, Transition } from '@headlessui/react'
import { Bars3Icon, BellIcon, XMarkIcon } from '@heroicons/react/24/outline'
import { useLoginContext } from '../hooks/useLoginContext'
import { useNavigate } from 'react-router-dom'

function classNames(...classes: any) {
  return classes.filter(Boolean).join(' ')
}

const NavBar = () => {
  const { loginResult, logout } = useLoginContext();
  const navigate = useNavigate();

  const handleSignOut = async (e: React.MouseEvent<HTMLElement>) => {
    e.preventDefault();
    if (logout) logout();
    navigate("/");
  }

  const user = loginResult?.user;
  const isAdmin = loginResult?.roles?.includes("ADMIN");

  return (
    <Disclosure as="nav" className="bg-green-800">
      {({ open }) => (
        <>
          <div className="mx-auto max-w-7xl px-2 sm:px-6 lg:px-8">
            <div className="relative flex h-16 items-center justify-between">
              <div className="absolute inset-y-0 left-0 flex items-center sm:hidden">
                {/* Mobile menu button*/}
                <Disclosure.Button className="relative inline-flex items-center justify-center rounded-md p-2 text-green-400 hover:bg-green-700 hover:text-white focus:outline-none focus:ring-2 focus:ring-inset focus:ring-white">
                  <span className="absolute -inset-0.5" />
                  <span className="sr-only">Open main menu</span>
                  {open ? (
                    <XMarkIcon className="block h-6 w-6" aria-hidden="true" />
                  ) : (
                    <Bars3Icon className="block h-6 w-6" aria-hidden="true" />
                  )}
                </Disclosure.Button>
              </div>
              <div className="flex flex-1 items-center justify-center sm:items-stretch sm:justify-start">
                <div className="flex flex-shrink-0 items-center">
                  <a href="/">
                    <img
                      className="h-8 w-auto"
                      src="logo.svg"
                      alt="Wingrid"
                    />
                  </a>
                </div>
                <div className="hidden sm:ml-6 sm:block">
                  <div className="flex space-x-4">
                    {user && <a

                      href="/dashboard"
                      className="text-green-300 hover:bg-green-700 hover:text-white rounded-md px-3 py-2 text-sm font-medium"
                    >
                      Dashboard
                    </a>}
                    {isAdmin && <a
                      href="/admin"
                      className="text-green-300 hover:bg-green-700 hover:text-white rounded-md px-3 py-2 text-sm font-medium"
                    >
                      Admin
                    </a>}
                    <a
                      href="/about"
                      className="text-green-300 hover:bg-green-700 hover:text-white rounded-md px-3 py-2 text-sm font-medium"
                    >
                      About
                    </a>
                  </div>
                </div>
              </div>
              {user && <div className="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0">
                <button
                  type="button"
                  className="relative rounded-full bg-green-800 p-1 text-green-400 hover:text-white focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-green-800"
                >
                  <span className="absolute -inset-1.5" />
                  <span className="sr-only">View notifications</span>
                  <BellIcon className="h-6 w-6" aria-hidden="true" />
                </button>

                {/* Profile dropdown */}
                <Menu as="div" className="relative ml-3">
                  <div>
                    <Menu.Button className="relative flex rounded-full bg-green-800 text-sm focus:outline-none focus:ring-2 focus:ring-white focus:ring-offset-2 focus:ring-offset-green-800">
                      <span className="absolute -inset-1.5" />
                      <span className="sr-only">Open user menu</span>
                      <img
                        className="h-8 w-8 rounded-full"
                        src="avatar.svg"
                        alt="Placeholder user avatar"
                      />
                    </Menu.Button>
                  </div>
                  <Transition
                    as={Fragment}
                    enter="transition ease-out duration-100"
                    enterFrom="transform opacity-0 scale-95"
                    enterTo="transform opacity-100 scale-100"
                    leave="transition ease-in duration-75"
                    leaveFrom="transform opacity-100 scale-100"
                    leaveTo="transform opacity-0 scale-95"
                  >
                    <Menu.Items className="absolute right-0 z-10 mt-2 w-48 origin-top-right rounded-md bg-white py-1 shadow-lg ring-1 ring-black ring-opacity-5 focus:outline-none">
                      <Menu.Item>
                        {({ active }) => (
                          <a
                            href="#"
                            className={classNames(active ? 'bg-green-100' : '', 'block px-4 py-2 text-sm text-green-700')}
                          >
                            Your Profile
                          </a>
                        )}
                      </Menu.Item>
                      <Menu.Item>
                        {({ active }) => (
                          <a
                            href="#"
                            className={classNames(active ? 'bg-green-100' : '', 'block px-4 py-2 text-sm text-green-700')}
                          >
                            Settings
                          </a>
                        )}
                      </Menu.Item>
                      <Menu.Item>
                        {({ active }) => (
                          <a
                            onClick={(e) => handleSignOut(e)}
                            className={classNames(active ? 'bg-green-100' : '', 'block px-4 py-2 text-sm text-green-700')}
                          >
                            Sign out
                          </a>
                        )}
                      </Menu.Item>
                    </Menu.Items>
                  </Transition>
                </Menu>
              </div>}
            </div>
          </div>

          <Disclosure.Panel className="sm:hidden">
            <div className="space-y-1 px-2 pb-3 pt-2">
              {user && <Disclosure.Button
                as="a"
                href="/dashboard"
                className='text-green-300 hover:bg-green-700 hover:text-white block rounded-md px-3 py-2 text-base font-medium'
              >
                Dashboard
              </Disclosure.Button>}
              {isAdmin && <Disclosure.Button
                as="a"
                href="/admin"
                className='text-green-300 hover:bg-green-700 hover:text-white block rounded-md px-3 py-2 text-base font-medium'
              >
                Admin
              </Disclosure.Button>}
              <Disclosure.Button
                as="a"
                href="/about"
                className='text-green-300 hover:bg-green-700 hover:text-white block rounded-md px-3 py-2 text-base font-medium'
              >
                About
              </Disclosure.Button>
            </div>
          </Disclosure.Panel>
        </>
      )}
    </Disclosure>
  )
}

export default NavBar;