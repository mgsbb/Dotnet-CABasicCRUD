import { useMutation } from "@tanstack/react-query";
import axios, { AxiosError } from "axios";
import { useState } from "react";
import { useParams } from "react-router";

// ===================================================================================================================
// ===================================================================================================================

type EditUserProfileFormData = {
  fullName: string;
  bio: string;
};

type EditUserProfileValidationError = Partial<EditUserProfileFormData>;

const initialFormData: EditUserProfileFormData = {
  fullName: "",
  bio: "",
};

const initialError: EditUserProfileValidationError = {
  fullName: undefined,
  bio: undefined,
};

// ===================================================================================================================
// ===================================================================================================================

export default function EditUserProfile() {
  const { id: userId } = useParams();

  const [formData, setFormData] =
    useState<EditUserProfileFormData>(initialFormData);

  const [errorMessage, setErrorMessage] = useState<string | null>(null);

  const [successMessage, setSuccessMessage] = useState<string | null>(null);

  const [validationErrors, setValidationErrors] =
    useState<EditUserProfileValidationError>(initialError);

  // -------------------------------------------------------------------------------------------------------------------

  const editUserProfileMutation = useMutation({
    mutationFn: async (data: EditUserProfileFormData) => {
      const response = await axios.patch(
        `/api/v1/users/${userId}/profile`,
        data,
        {
          withCredentials: true,
        },
      );

      return response.data;
    },

    onSuccess: () => {
      setSuccessMessage("User edited successfully.");
      setErrorMessage(null);
    },

    onError: (err: AxiosError) => {
      console.error(err.response?.data);

      if (err.response?.status == 403) {
        setErrorMessage("Cannot edit another user.");
        setSuccessMessage(null);
      }
    },
  });

  // -------------------------------------------------------------------------------------------------------------------

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    let isReturn = false;

    if (formData.fullName === "") {
      setValidationErrors((prev) => ({
        ...prev,
        fullName: "Full name is required.",
      }));
      isReturn = true;
    } else {
      setValidationErrors((prev) => ({ ...prev, fullName: undefined }));
    }

    if (formData.bio === "") {
      setValidationErrors((prev) => ({
        ...prev,
        bio: "Bio is required.",
      }));
      isReturn = true;
    } else {
      setValidationErrors((prev) => ({ ...prev, bio: undefined }));
    }

    console.log(validationErrors);

    if (isReturn) return;

    editUserProfileMutation.mutate(formData);
  };

  // -------------------------------------------------------------------------------------------------------------------

  const handleChange = (
    e:
      | React.ChangeEvent<HTMLInputElement>
      | React.ChangeEvent<HTMLTextAreaElement>,
  ) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  // -------------------------------------------------------------------------------------------------------------------

  return (
    <form
      onSubmit={handleSubmit}
      className="flex flex-col gap-10 w-3/4 mx-auto"
    >
      <h2 className="font-semibold text-2xl">User Profile</h2>

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
        <label htmlFor="fullName" className="font-medium text-gray-500">
          Name
        </label>
        <input
          name="fullName"
          id="fullName"
          value={formData.fullName}
          onChange={handleChange}
          className="border border-gray-300 rounded-sm p-2"
        />
        {validationErrors.fullName !== undefined && (
          <span className="text-sm font-semibold text-red-600">
            {validationErrors.fullName}
          </span>
        )}
      </div>

      <div className="flex flex-col gap-1">
        <label htmlFor="bio" className="font-medium text-gray-500">
          Bio
        </label>
        <textarea
          name="bio"
          id="bio"
          value={formData.bio}
          onChange={handleChange}
          className="border border-gray-300 rounded-sm p-2"
          rows={5}
        ></textarea>
        {validationErrors.bio !== undefined && (
          <span className="text-sm font-semibold text-red-600">
            {validationErrors.bio}
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
