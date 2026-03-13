import { Link } from "react-router";

export default function Landing() {
  return (
    <section className="p-10 flex flex-col items-center">
      <h1 className="text-6xl">Welcome</h1>

      <p className="mt-2">This is the landing page.</p>

      <nav className="mt-10 flex gap-10 items-center">
        <Link
          to={"/auth/register"}
          className="bg-black py-2 px-4 text-gray-300 rounded-sm cursor-pointer"
        >
          Create an account
        </Link>
        <Link
          to={"/auth/login"}
          className="bg-white border-black border py-2 px-4 rounded-sm cursor-pointer"
        >
          Login
        </Link>
      </nav>
    </section>
  );
}
