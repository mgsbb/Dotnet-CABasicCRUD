import { useMutation } from "@tanstack/react-query";
import axios, { AxiosError } from "axios";
import { useState } from "react";
import { useParams } from "react-router";

export default function EditUserEmail() {
  const { id: userId } = useParams();

  const [email, setEmail] = useState("");

  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const [validationError, setValidationError] = useState<string | undefined>(
    undefined,
  );

  const editUserEmailMutation = useMutation({
    mutationFn: async (email: string) => {
      const response = await axios.patch(
        `/api/v1/users/${userId}/email`,
        { email },
        {
          withCredentials: true,
        },
      );

      return response.data;
    },

    onSuccess: () => {
      setSuccessMessage("User email changed successfully.");
      setErrorMessage(null);
    },

    onError: (err: AxiosError) => {
      console.error(err.response?.data);

      if (err.response?.status == 403) {
        setErrorMessage("Cannot edit another user.");
        setSuccessMessage(null);
      }

      if (err.response?.status == 409) {
        setErrorMessage("Email already taken.");
        setSuccessMessage(null);
      }
    },
  });

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    let isReturn = false;

    if (email === "") {
      setValidationError("Email is required");
      isReturn = true;
    } else {
      setValidationError(undefined);
    }

    if (isReturn) return;

    editUserEmailMutation.mutate(email);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setEmail(e.target.value);
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="flex flex-col gap-10 w-3/4 mx-auto"
    >
      <h2 className="font-semibold text-2xl">User Email</h2>

      {errorMessage !== null && (
        <div className="bg-red-100 text-red-700 font-semibold px-4 py-2 rounded-sm text-center">
          {errorMessage}
        </div>
      )}

      {successMessage !== null && (
        <div className="bg-green-100 text-green-700 font-semibold px-4 py-2 rounded-sm text-center">
          {successMessage}
        </div>
      )}

      <div className="flex flex-col gap-1">
        <label htmlFor="email" className="font-medium text-gray-500">
          Email
        </label>
        <input
          name="email"
          id="email"
          value={email}
          onChange={handleChange}
          className="border border-gray-300 rounded-sm p-2"
        />
        {validationError !== undefined && (
          <span className="text-sm font-semibold text-red-600">
            {validationError}
          </span>
        )}
      </div>

      <button
        type="submit"
        className="bg-gray-900 text-white py-3 rounded-sm cursor-pointer font-semibold text-sm"
      >
        Edit
      </button>
    </form>
  );
}
