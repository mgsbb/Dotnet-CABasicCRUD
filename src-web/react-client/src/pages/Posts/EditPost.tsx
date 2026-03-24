import { useMutation } from "@tanstack/react-query";
import axios from "axios";
import { useState } from "react";
import { useNavigate, useParams } from "react-router";

type EditPostFormData = {
  title: string;
  content: string;
};

const initialState: EditPostFormData = {
  title: "",
  content: "",
};

export default function EditPost() {
  const { id: postId } = useParams();

  const [formData, setFormData] = useState<EditPostFormData>(initialState);

  const navigate = useNavigate();

  const editPostMutation = useMutation({
    mutationFn: async (formData: EditPostFormData) => {
      // send null if empty string or whitespace
      const data = {
        title: formData.title !== "" ? formData.title : null,
        content: formData.content !== "" ? formData.content : null,
      };

      const response = await axios.patch(`/api/v1/posts/${postId}`, data, {
        withCredentials: true,
      });

      return response.data;
    },

    onSuccess: () => {
      navigate(`/posts/${postId}`);
    },

    onError: (err: any) => {
      console.error(err);
    },
  });

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    editPostMutation.mutate(formData);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  return (
    <section className="flex flex-col gap-6">
      <h1 className="text-3xl font-medium text-gray-700 text-center">
        Edit Post
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
          className="bg-gray-900 text-white py-3 rounded-sm cursor-pointer font-semibold text-sm"
        >
          Edit
        </button>
      </form>
    </section>
  );
}
