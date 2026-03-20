import { Link } from "react-router";

export default function Home() {
  return (
    <section className="flex items-center flex-col">
      <h1 className="font-bold text-3xl">Home</h1>

      <nav className="mt-8 flex items-center gap-4">
        <Link to="/posts">
          <button className="bg-gray-800 text-white py-2 px-4 rounded-sm cursor-pointer text-sm">
            View posts
          </button>
        </Link>

        <Link to="/users">
          <button className="bg-gray-800 text-white py-2 px-4 rounded-sm cursor-pointer text-sm">
            View users
          </button>
        </Link>
      </nav>
    </section>
  );
}
