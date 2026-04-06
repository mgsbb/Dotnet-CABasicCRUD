import { Outlet } from "react-router";
import Navbar from "./components/Navbar";
import Sidebar from "./components/Sidebar";
import { useState } from "react";

export default function ChatLayout({
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
        className={` min-h-screen ${isSidebarOpen ? "lg:ml-80" : ""}`}
      >
        {/* backdrop */}
        <div
          className={`${isSidebarOpen ? "lg:hidden bg-black/30 fixed top-0 left-0 w-screen h-screen " : ""}`}
        ></div>
        <div className={`mx-auto min-h-screen px-4 w-full 2xl:w-2/3`}>
          {children ? <main>{children}</main> : <Outlet />}
        </div>
      </main>
    </>
  );
}
