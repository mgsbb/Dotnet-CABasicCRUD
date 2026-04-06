import { useMutation } from "@tanstack/react-query";
import axios, { AxiosError } from "axios";
import { useState } from "react";
import { useNavigate } from "react-router";
import type { TPost } from "../../types/posts";

// ===================================================================================================================
// ===================================================================================================================

type CreatePostFormData = {
  title: string;
  content: string;
};

type CreatePostValidationError = Partial<CreatePostFormData>;

const initialState: CreatePostFormData = {
  title: "",
  content: "",
};

const initialError: CreatePostValidationError = {
  title: undefined,
  content: undefined,
};

// ===================================================================================================================
// ===================================================================================================================

export default function () {
  const [formData, setFormData] = useState<CreatePostFormData>(initialState);

  const [errors, setErrors] = useState<CreatePostValidationError>(initialError);

  const navigate = useNavigate();

  // -------------------------------------------------------------------------------------------------------------------

  const createPostMutation = useMutation({
    mutationFn: async (data: CreatePostFormData): Promise<TPost> => {
      const response = await axios.post("/api/v1/posts", data, {
        withCredentials: true,
      });

      return response.data;
    },

    onSuccess: (data) => {
      navigate(`/posts/${data.id}`);
    },

    onError: (err: AxiosError) => {
      console.error(err.response?.data);
    },
  });

  // -------------------------------------------------------------------------------------------------------------------

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    let isReturn = false;

    if (formData.title === "") {
      setErrors((prev) => ({ ...prev, title: "Title is required." }));
      isReturn = true;
    } else {
      setErrors((prev) => ({ ...prev, title: undefined }));
    }

    if (formData.content === "") {
      setErrors((prev) => ({ ...prev, content: "Content is required." }));
      isReturn = true;
    } else {
      setErrors((prev) => ({ ...prev, content: undefined }));
    }

    if (isReturn) return;

    createPostMutation.mutate(formData);
  };

  // -------------------------------------------------------------------------------------------------------------------

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  // -------------------------------------------------------------------------------------------------------------------

  return (
    <section className="flex flex-col gap-16">
      <h1 className="text-3xl font-medium text-gray-700 text-center">
        Create Post
      </h1>

      <form
        onSubmit={handleSubmit}
        className="flex flex-col gap-10 w-5/6 lg:w-3/4 mx-auto"
      >
        <div className="flex flex-col gap-1">
          <label
            htmlFor="title"
            className="font-medium text-gray-500 flex flex-col gap-1"
          >
            Title
            <input
              id="title"
              name="title"
              value={formData.title}
              onChange={handleChange}
              className="border border-gray-300 rounded-sm p-2 text-black font-normal"
            />
          </label>
          {errors.title && (
            <span className="text-sm font-semibold text-red-600">
              {errors.title}
            </span>
          )}
        </div>

        <div className="flex flex-col gap-1">
          <label
            htmlFor="Content"
            className="font-medium text-gray-500 flex flex-col gap-1"
          >
            Content
            <textarea
              id="content"
              name="content"
              value={formData.content}
              onChange={(e) => {
                setFormData({ ...formData, [e.target.name]: e.target.value });
              }}
              className="border border-gray-300 rounded-sm p-2 text-black font-normal"
              rows={15}
            ></textarea>
          </label>
          {errors.content && (
            <span className="text-sm font-semibold text-red-600">
              {errors.content}
            </span>
          )}
        </div>

        <button
          type="submit"
          className="bg-gray-900 text-white py-3 px-4 rounded-sm cursor-pointer font-semibold text-sm"
        >
          Create
        </button>
      </form>
    </section>
  );
}
