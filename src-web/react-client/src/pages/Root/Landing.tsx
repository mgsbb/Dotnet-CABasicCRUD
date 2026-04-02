import { Link } from "react-router";

export default function LandingPage() {
  return (
    <div className="min-h-screen bg-white text-gray-800">
      {/* navbar */}
      <header>
        <nav className="flex justify-between items-center px-8 py-6 border-b border-gray-200">
          <h1 className="text-2xl font-bold text-violet-600">Connectly</h1>

          <div className="space-x-6 hidden md:flex">
            <Link to="#features" className="hover:text-violet-600">
              Features
            </Link>
            <Link to="#about" className="hover:text-violet-600">
              About
            </Link>
            <Link to="#join" className="hover:text-violet-600">
              Join
            </Link>
          </div>

          <Link to={"/auth/register"}>
            <button className="bg-violet-600 hover:bg-violet-700 cursor-pointer text-white px-5 py-2 rounded-sm">
              Sign Up
            </button>
          </Link>
        </nav>
      </header>

      {/* hero section */}
      <section className="px-8 py-24 text-center bg-gray-50">
        <h2 className="text-5xl font-bold mb-6">Share Your World Instantly</h2>

        <p className="text-lg text-gray-600 max-w-2xl mx-auto mb-8">
          Connect with friends, discover communities, and share moments that
          matter. Connectly brings people together in a fast and beautiful
          social experience.
        </p>

        <div className="space-x-4">
          <Link to={"/auth/register"}>
            <button className="bg-violet-600 text-white px-6 py-3 rounded-sm hover:bg-violet-700  cursor-pointer">
              Get Started
            </button>
          </Link>
          <button className="border-gray-400 border text-gray-500 px-6 py-3 rounded-sm hover:bg-gray-100 cursor-pointer">
            Learn More
          </button>
        </div>
      </section>

      {/* features */}
      <section id="features" className="px-8 py-24 max-w-6xl mx-auto">
        <h3 className="text-3xl font-bold text-center mb-16">
          Why Choose Connectly
        </h3>

        <div className="grid md:grid-cols-3 gap-10">
          <div className="p-6 border border-gray-300 rounded-md hover:shadow-md transition">
            <h4 className="text-xl font-semibold mb-3">Real-Time Chat</h4>
            <p className="text-gray-600">
              Stay connected with instant messaging, reactions, and media
              sharing with your friends and communities.
            </p>
          </div>

          <div className="p-6 border border-gray-300 rounded-md hover:shadow-md transition">
            <h4 className="text-xl font-semibold mb-3">Discover Communities</h4>
            <p className="text-gray-600">
              Join groups based on your interests and discover trending topics
              around the world.
            </p>
          </div>

          <div className="p-6 border border-gray-300 rounded-md hover:shadow-md transition">
            <h4 className="text-xl font-semibold mb-3">Share Moments</h4>
            <p className="text-gray-600">
              Post photos, videos, and stories to keep everyone updated with
              what matters to you.
            </p>
          </div>
        </div>
      </section>

      {/* app details */}
      <section className="bg-gray-50 py-24 px-8">
        <div className="max-w-6xl mx-auto grid md:grid-cols-2 gap-16 items-center">
          <div>
            <h3 className="text-3xl font-bold mb-6">
              Built for Modern Social Interaction
            </h3>

            <p className="text-gray-600 mb-6">
              Connectly is designed to be fast, intuitive, and engaging. Whether
              you're sharing photos, messaging friends, or discovering new
              communities, everything feels effortless.
            </p>

            <ul className="space-y-3 text-gray-700">
              <li>✔ Lightning fast feed</li>
              <li>✔ Smart recommendations</li>
              <li>✔ Privacy-first design</li>
              <li>✔ Mobile-first experience</li>
            </ul>
          </div>

          <div className="bg-white border border-gray-300 rounded-xl p-8 shadow-md">
            <div className="space-y-4">
              <div className="h-4 bg-gray-200 rounded"></div>
              <div className="h-4 bg-gray-200 rounded w-5/6"></div>
              <div className="h-40 bg-gray-200 rounded"></div>
              <div className="h-4 bg-gray-200 rounded w-2/3"></div>
            </div>
          </div>
        </div>
      </section>

      {/* call to action */}
      <section id="join" className="px-8 py-24 text-center bg-white text-black">
        <h3 className="text-3xl font-bold mb-6">
          Join Millions Sharing Their Stories
        </h3>

        <p className="mb-8 text-black">
          Create your account today and start connecting with the world.
        </p>

        <Link to={"/auth/register"}>
          <button className="bg-violet-600 hover:bg-violet-700 cursor-pointer text-white px-5 py-2 rounded-sm">
            Create Free Account
          </button>
        </Link>
      </section>

      {/* footer */}
      <footer className="text-center bg-gray-50 py-10 border-t border-gray-200 text-gray-500 text-sm">
        © {new Date().getFullYear()} Connectly. All rights reserved.
      </footer>
    </div>
  );
}
