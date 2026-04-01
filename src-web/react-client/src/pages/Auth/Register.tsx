import { useMutation } from "@tanstack/react-query";
import axios, { AxiosError } from "axios";
import React, { useState } from "react";
import { Link, useNavigate } from "react-router";
import AuthInput from "./AuthInput";
import type {
  BadRequestResponse,
  ConflictResponse,
} from "../../types/ApiErrorResponse";
import { toast, Toaster } from "sonner";

type RegisterFormData = {
  email: string;
  password: string;
  name: string;
  username: string;
};

type RegisterValidationError = Partial<RegisterFormData>;

const initialState: RegisterFormData = {
  email: "",
  password: "",
  name: "",
  username: "",
};

const initialValidationErrors: RegisterValidationError = {
  email: undefined,
  password: undefined,
  name: undefined,
  username: undefined,
};

export default function Register() {
  const navigate = useNavigate();

  const [formData, setFormData] = useState<RegisterFormData>(initialState);

  const [validationErrors, setValidationErrors] =
    useState<RegisterValidationError>(initialValidationErrors);

  const registerMutation = useMutation({
    mutationFn: async (data: RegisterFormData) => {
      const response = await axios.post("/api/v1/auth/register", data, {
        withCredentials: true,
      });

      return response.data;
    },

    onSuccess: () => {
      toast.error("Registered successfully", {
        className: "!bg-green-100 !text-green-700 !text-base",
      });
      navigate("/");
    },

    onError(
      error: AxiosError<BadRequestResponse> | AxiosError<ConflictResponse>,
    ) {
      const data = error.response?.data;

      if (error.response?.status === 400 && data && "errors" in data) {
        data.errors?.map((e) => {
          setValidationErrors((prev) => {
            const code: keyof RegisterValidationError =
              e.code.toLowerCase() as keyof RegisterValidationError;
            return {
              ...prev,
              [code]: `${prev[code] ? prev[code] : ""} ${e.message}`,
            };
          });
        });
      }

      if (error.response?.status == 409) {
        toast.error("Email already exists", {
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

    if (formData.name === "") {
      setValidationErrors((prev) => ({ ...prev, name: "Name is required." }));
      isReturn = true;
    } else {
      setValidationErrors((prev) => ({ ...prev, name: undefined }));
    }

    if (formData.username === "") {
      setValidationErrors((prev) => ({
        ...prev,
        username: "Username is required.",
      }));
      isReturn = true;
    } else {
      setValidationErrors((prev) => ({ ...prev, username: undefined }));
    }

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

    registerMutation.mutate(formData);
  };

  return (
    <div className="flex min-h-screen">
      <Toaster position="top-center" />

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
                validationError={validationErrors.name}
              />

              <AuthInput
                label="Username"
                type="text"
                id="username"
                value={formData.username}
                onChange={handleChange}
                validationError={validationErrors.username}
              />

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
              Register
            </button>
          </form>
        </div>
      </section>
    </div>
  );
}
