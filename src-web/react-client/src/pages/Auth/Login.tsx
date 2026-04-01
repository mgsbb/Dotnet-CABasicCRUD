import { useMutation } from "@tanstack/react-query";
import axios, { AxiosError } from "axios";
import React, { useState } from "react";
import { Link, useNavigate } from "react-router";
import AuthInput from "./AuthInput";
import { toast, Toaster } from "sonner";
import type { UnauthorizedResponse } from "../../types/ApiErrorResponse";

type LoginFormData = {
  email: string;
  password: string;
};

type LoginValidationError = Partial<LoginFormData>;

const initialState: LoginFormData = {
  email: "",
  password: "",
};

const initialValidationErrors: LoginValidationError = {
  email: "",
  password: "",
};

export default function Login() {
  const navigate = useNavigate();

  const [formData, setFormData] = useState<LoginFormData>(initialState);

  const [validationErrors, setValidationErrors] =
    useState<LoginValidationError>(initialValidationErrors);

  const loginMutation = useMutation({
    mutationFn: async (data: LoginFormData) => {
      const response = await axios.post("/api/v1/auth/login", data, {
        withCredentials: true,
      });
      return response.data;
    },

    onSuccess: () => {
      toast.success("Login success. Redirecting...", {
        className: "!bg-green-100 !text-green-700 !text-base",
      });
      setTimeout(() => {
        navigate("/");
      }, 1000);
    },

    onError(error: AxiosError<UnauthorizedResponse>) {
      if (error.response?.status == 401) {
        toast.error(error.response?.data.detail, {
          className: "!bg-red-100 !text-red-700 !text-base",
        });
      }
    },
  });

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    let isReturn = false;

    if (formData.email === "") {
      setValidationErrors((prev) => ({ ...prev, email: "Email is required." }));
      isReturn = true;
    } else {
      setValidationErrors((prev) => ({ ...prev, email: undefined }));
    }

    if (formData.password === "") {
      setValidationErrors((prev) => ({
        ...prev,
        password: "Password is required.",
      }));
      isReturn = true;
    } else {
      setValidationErrors((prev) => ({ ...prev, password: undefined }));
    }

    if (isReturn) return;

    loginMutation.mutate(formData);
  };

  return (
    <div className="flex min-h-screen">
      <Toaster position="top-center" />

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
              <AuthInput
                label="Email"
                type="email"
                id="email"
                value={formData.email}
                onChange={handleChange}
                validationError={validationErrors.email}
              />

              <AuthInput
                label="Password"
                type="password"
                id="password"
                value={formData.password}
                onChange={handleChange}
                validationError={validationErrors.password}
              />
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
