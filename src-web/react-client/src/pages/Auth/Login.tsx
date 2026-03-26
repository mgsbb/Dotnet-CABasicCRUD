import { useMutation } from "@tanstack/react-query";
import axios from "axios";
import React, { useState } from "react";
import { Link, useNavigate } from "react-router";

type LoginFormData = {
  email: string;
  password: string;
};

const initialState: LoginFormData = {
  email: "",
  password: "",
};

export default function Login() {
  const navigate = useNavigate();

  const [formData, setFormData] = useState<LoginFormData>(initialState);

  const loginMutation = useMutation({
    mutationFn: async (data: LoginFormData) => {
      const response = await axios.post("/api/v1/auth/login", data, {
        withCredentials: true,
      });
      //   console.log(response.data);
      return response.data;
    },

    onSuccess: () => {
      // console.log("Login successful");
      navigate("/");
    },

    onError(error: any) {
      console.error(error);
    },
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    loginMutation.mutate(formData);
  };

  return (
    <div className="flex min-h-screen">
      <section className="hidden lg:block bg-gray-800 flex-1"></section>

      <section className="flex-1 pt-40">
        <div className="mx-auto w-3/4 sm:w-2/3 xl:w-1/2">
          <h1 className="text-5xl font-medium text-gray-700 mb-4">Login</h1>

          <Link to="/auth/register" className="text-gray-500 text-center">
            Don't have an account? Create one
          </Link>

          <hr className="border border-gray-300 mb-12 mt-4" />

          <form
            onSubmit={handleSubmit}
            className="text-black flex flex-col gap-10"
          >
            <div className="flex flex-col gap-4">
              <label
                htmlFor="email"
                className="font-medium text-gray-500 flex flex-col gap-1"
              >
                Email
                <input
                  type="email"
                  id="email"
                  name="email"
                  value={formData.email}
                  onChange={handleChange}
                  className="border border-gray-300 rounded-sm p-2 text-black font-normal"
                />
              </label>

              <label
                htmlFor="password"
                className="font-medium text-gray-500 flex flex-col gap-1"
              >
                Password
                <input
                  type="password"
                  id="password"
                  name="password"
                  value={formData.password}
                  onChange={handleChange}
                  className="border border-gray-300 rounded-sm p-2 text-black font-normal"
                />
              </label>
            </div>

            <button
              type="submit"
              className="bg-gray-900 text-white py-3 rounded-sm cursor-pointer font-semibold text-sm"
            >
              Login
            </button>
          </form>
        </div>
      </section>
    </div>
  );
}
