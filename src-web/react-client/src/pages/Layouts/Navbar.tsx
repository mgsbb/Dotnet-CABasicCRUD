export default function Navbar() {
  return (
    <header
      id="header"
      className="fixed top-0 h-16 w-full bg-white border-b border-gray-300 z-10 p-4"
    >
      <nav>
        <button
          type="button"
          // onclick="toggleSidebar()"
          className="text-gray-500 cursor-pointer md:hidden"
        >
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            strokeWidth="1.5"
            stroke="currentColor"
            className="size-8"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M3.75 6.75h16.5M3.75 12h16.5m-16.5 5.25h16.5"
            />
          </svg>
        </button>
      </nav>
    </header>
  );
}
