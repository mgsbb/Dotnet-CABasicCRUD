import { useMutation } from "@tanstack/react-query";
import axios from "axios";
import { useState } from "react";
import { useNavigate } from "react-router";

type CreatePostFormData = {
  title: string;
  content: string;
};

const initialState: CreatePostFormData = {
  title: "",
  content: "",
};

export default function () {
  const [formData, setFormData] = useState<CreatePostFormData>(initialState);

  const navigate = useNavigate();

  const createPostMutation = useMutation({
    mutationFn: async (data: CreatePostFormData) => {
      const response = await axios.post("/api/v1/posts", data, {
        withCredentials: true,
      });

      return response.data;
    },

    onSuccess: (data) => {
      navigate(`/posts/${data.id}`);
    },

    onError: (err: any) => {
      console.error(err);
    },
  });

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    createPostMutation.mutate(formData);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  return (
    <section className="flex flex-col gap-16">
      <h1 className="text-3xl font-medium text-gray-700 text-center">
        Create Post
      </h1>

      <form
        onSubmit={handleSubmit}
        className="flex flex-col gap-10 w-5/6 lg:w-3/4 mx-auto"
      >
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
            className="border border-gray-300 rounded-sm p-2"
          />
        </label>

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
            className="border border-gray-300 rounded-sm p-2"
            rows={15}
          ></textarea>
        </label>

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
