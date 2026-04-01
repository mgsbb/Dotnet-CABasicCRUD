import { useMutation } from "@tanstack/react-query";
import axios, { AxiosError } from "axios";
import { useState } from "react";
import { useParams } from "react-router";
import type {
  BadRequestResponse,
  UnauthorizedResponse,
} from "../../types/ApiErrorResponse";

function pascalToCamel(input: string): string {
  if (!input) return "";
  return input[0].toLowerCase() + input.slice(1);
}

type EditUserPasswordFormData = {
  oldPassword: string;
  newPassword: string;
  newPasswordConfirmed: string;
};

type EditUserPasswordValidationError = Partial<EditUserPasswordFormData>;

const initialFormData: EditUserPasswordFormData = {
  oldPassword: "",
  newPassword: "",
  newPasswordConfirmed: "",
};

const initialError: EditUserPasswordValidationError = {
  oldPassword: undefined,
  newPassword: undefined,
  newPasswordConfirmed: undefined,
};

export default function EditUserPassword() {
  const { id: userId } = useParams();

  const [formData, setFormData] =
    useState<EditUserPasswordFormData>(initialFormData);

  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const [validationErrors, setValidationErrors] =
    useState<EditUserPasswordValidationError>(initialError);

  const editUserPasswordMutation = useMutation({
    mutationFn: async (data: EditUserPasswordFormData) => {
      const response = await axios.patch(
        `/api/v1/users/${userId}/password`,
        data,
        {
          withCredentials: true,
        },
      );

      return response.data;
    },

    onSuccess: () => {
      setSuccessMessage("User password changed successfully.");
      setErrorMessage(null);
    },

    onError: (
      err: AxiosError<UnauthorizedResponse> | AxiosError<BadRequestResponse>,
    ) => {
      const data = err.response?.data;

      if (err.response?.status === 400 && data && "errors" in data) {
        data.errors?.map((e) => {
          setValidationErrors((prev) => {
            const code: keyof EditUserPasswordValidationError = pascalToCamel(
              e.code,
            ) as keyof EditUserPasswordValidationError;
            return {
              ...prev,
              [code]: `${prev[code] ? prev[code] : ""} ${e.message}`,
            };
          });
        });
      }

      if (err.response?.status === 403) {
        setErrorMessage(err.response?.data.detail);
        setSuccessMessage(null);
      }

      console.log(validationErrors);
    },
  });

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    let isReturn = false;

    if (formData.oldPassword === "") {
      setValidationErrors((prev) => ({
        ...prev,
        oldPassword: "Old password is required.",
      }));
      isReturn = true;
    } else {
      setValidationErrors((prev) => ({ ...prev, oldPassword: undefined }));
    }

    if (formData.newPassword === "") {
      setValidationErrors((prev) => ({
        ...prev,
        newPassword: "New password is required.",
      }));
      isReturn = true;
    } else {
      setValidationErrors((prev) => ({ ...prev, newPassword: undefined }));
    }

    if (formData.newPasswordConfirmed === "") {
      setValidationErrors((prev) => ({
        ...prev,
        newPasswordConfirmed: "New password confirmation is required.",
      }));
      isReturn = true;
    } else {
      setValidationErrors((prev) => ({
        ...prev,
        newPasswordConfirmed: undefined,
      }));
    }

    if (formData.newPassword !== formData.newPasswordConfirmed) {
      setErrorMessage(() => "Passwords don't match");
      isReturn = true;
    } else {
      setErrorMessage(null);
    }

    if (isReturn) return;

    editUserPasswordMutation.mutate(formData);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  return (
    <form
      onSubmit={handleSubmit}
      className="flex flex-col gap-10 w-3/4 mx-auto"
    >
      <h2 className="font-semibold text-2xl">User Password</h2>

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
        <label htmlFor="oldPassword" className="font-medium text-gray-500">
          Old Password
        </label>
        <input
          name="oldPassword"
          id="oldPassword"
          type="password"
          value={formData.oldPassword}
          onChange={handleChange}
          className="border border-gray-300 rounded-sm p-2"
        />
        {validationErrors.oldPassword !== undefined && (
          <span className="text-sm font-semibold text-red-600">
            {validationErrors.oldPassword}
          </span>
        )}
      </div>

      <div className="flex flex-col gap-1">
        <label htmlFor="newPassword" className="font-medium text-gray-500">
          New Password
        </label>
        <input
          name="newPassword"
          id="newPassword"
          type="password"
          value={formData.newPassword}
          onChange={handleChange}
          className="border border-gray-300 rounded-sm p-2"
        />
        {validationErrors.newPassword !== undefined && (
          <span className="text-sm font-semibold text-red-600">
            {validationErrors.newPassword}
          </span>
        )}
      </div>

      <div className="flex flex-col gap-1">
        <label
          htmlFor="newPasswordConfirmed"
          className="font-medium text-gray-500"
        >
          Confirm New Password
        </label>
        <input
          name="newPasswordConfirmed"
          id="newPasswordConfirmed"
          type="password"
          value={formData.newPasswordConfirmed}
          onChange={handleChange}
          className="border border-gray-300 rounded-sm p-2"
        />
        {validationErrors.newPasswordConfirmed !== undefined && (
          <span className="text-sm font-semibold text-red-600">
            {validationErrors.newPasswordConfirmed}
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
