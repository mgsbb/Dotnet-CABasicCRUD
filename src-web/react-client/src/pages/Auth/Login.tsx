import { useMutation } from "@tanstack/react-query";
import axios from "axios";
import React, { useState } from "react";

type LoginFormData = {
  email: string;
  password: string;
};

const initialState: LoginFormData = {
  email: "",
  password: "",
};

export default function Login() {
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
      console.log("Login successful");
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
    <form onSubmit={handleSubmit} className="text-black">
      <label htmlFor="email">
        Email
        <input
          type="text"
          id="email"
          name="email"
          value={formData.email}
          onChange={handleChange}
          className="border"
        />
      </label>

      <label htmlFor="email">
        Password
        <input
          type="password"
          id="password"
          name="password"
          value={formData.password}
          onChange={handleChange}
          className="border"
        />
      </label>

      <button type="submit">Submit</button>
    </form>
  );
}
