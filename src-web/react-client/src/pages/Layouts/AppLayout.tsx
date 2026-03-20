import { Outlet } from "react-router";
import Navbar from "./Navbar";
import Sidebar from "./Sidebar";

export default function AppLayout({
  children,
}: {
  children?: React.ReactNode;
}) {
  return (
    <>
      <Sidebar />

      <Navbar />

      <main id="main" className="bg-gray-200 min-h-screen ml-80">
        <div className="mx-auto min-h-screen bg-gray-50 pt-24 px-4 w-full 2xl:w-2/3">
          {children ? <main>{children}</main> : <Outlet />}
        </div>
      </main>
    </>
  );
}
