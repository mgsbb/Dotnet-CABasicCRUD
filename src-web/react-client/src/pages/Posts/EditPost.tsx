import { useMutation } from "@tanstack/react-query";
import axios, { AxiosError } from "axios";
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

  const [errorMessage, setErrorMessage] = useState("");

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

    onError: (err: AxiosError) => {
      console.error(err.response?.data);

      if (err.response?.status === 403)
        setErrorMessage("Cannot edit the post of another user.");
    },
  });

  const handleSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    if (formData.title === "" && formData.content === "") {
      setErrorMessage("At least one field is required.");
      return;
    }

    editPostMutation.mutate(formData);
  };

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  return (
    <section className="flex flex-col gap-6 ">
      <h1 className="text-3xl font-medium text-gray-700 text-center">
        Edit Post
      </h1>

      {errorMessage !== "" && (
        <div className="bg-red-100 text-red-700 font-semibold px-4 py-2 mb-10 rounded-sm w-5/6 lg:w-3/4 mx-auto text-center">
          {errorMessage}
        </div>
      )}

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
            className="border border-gray-300 rounded-sm p-2 text-black font-normal"
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
            className="border border-gray-300 rounded-sm p-2 text-black font-normal"
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
