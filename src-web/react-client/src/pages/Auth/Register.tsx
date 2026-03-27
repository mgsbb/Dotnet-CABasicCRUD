import { useMutation } from "@tanstack/react-query";
import axios from "axios";
import React, { useState } from "react";
import { Link, useNavigate } from "react-router";
import AuthInput from "./AuthInput";

type RegisterFormData = {
  email: string;
  password: string;
  name: string;
  username: string;
};

const initialState: RegisterFormData = {
  email: "",
  password: "",
  name: "",
  username: "",
};

export default function Register() {
  const navigate = useNavigate();

  const [formData, setFormData] = useState<RegisterFormData>(initialState);

  const registerMutation = useMutation({
    mutationFn: async (data: RegisterFormData) => {
      const response = await axios.post("/api/v1/auth/register", data, {
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

    registerMutation.mutate(formData);
  };

  return (
    <div className="flex min-h-screen">
      <section className="hidden lg:block bg-gray-800 flex-1"></section>

      <section className="flex-1 pt-40">
        <div className="mx-auto w-3/4 sm:w-2/3 xl:w-1/2">
          <h1 className="text-5xl font-medium text-gray-700 mb-4">
            Create an account
          </h1>

          <Link to="/auth/login" className="text-gray-500 text-center">
            Already have an account? Login
          </Link>

          <hr className="border border-gray-300 mb-12 mt-4" />

          <form
            onSubmit={handleSubmit}
            className="text-black flex flex-col gap-10"
          >
            <div className="flex flex-col gap-4">
              <AuthInput
                label="Name"
                type="text"
                id="name"
                value={formData.name}
                onChange={handleChange}
              />

              <AuthInput
                label="Username"
                type="text"
                id="username"
                value={formData.username}
                onChange={handleChange}
              />

              <AuthInput
                label="Email"
                type="email"
                id="email"
                value={formData.email}
                onChange={handleChange}
              />

              <AuthInput
                label="Password"
                type="password"
                id="password"
                value={formData.password}
                onChange={handleChange}
              />
            </div>

            <button
              type="submit"
              className="bg-gray-900 text-white py-3 rounded-sm cursor-pointer font-semibold text-sm"
            >
              Register
            </button>
          </form>
        </div>
      </section>
    </div>
  );
}
