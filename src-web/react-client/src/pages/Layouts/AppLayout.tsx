import { Outlet } from "react-router";
import Navbar from "./Navbar";
import Sidebar from "./Sidebar";
import { useState } from "react";

export default function AppLayout({
  children,
}: {
  children?: React.ReactNode;
}) {
  const [isSidebarOpen, setIsSidebarOpen] = useState(true);

  return (
    <>
      <Sidebar
        isSidebarOpen={isSidebarOpen}
        setIsSidebarOpen={setIsSidebarOpen}
      />

      <Navbar
        isSidebarOpen={isSidebarOpen}
        setIsSidebarOpen={setIsSidebarOpen}
      />

      <main
        id="main"
        className={`bg-gray-200 min-h-screen ${isSidebarOpen ? "md:ml-48 lg:ml-80" : ""}`}
      >
        {/* backdrop */}
        <div
          className={`${isSidebarOpen ? "md:hidden bg-black/30 fixed top-0 left-0 w-screen h-screen " : ""}`}
        ></div>
        <div
          className={`mx-auto min-h-screen bg-gray-50 pt-24 px-4 w-full 2xl:w-2/3`}
        >
          {children ? <main>{children}</main> : <Outlet />}
        </div>
      </main>
    </>
  );
}
