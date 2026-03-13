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

      <main>{children}</main>
    </>
  );
}
