import { useMutation } from "@tanstack/react-query";
import axios, { AxiosError } from "axios";
import { useRef, useState } from "react";
import { useNavigate } from "react-router";
import type { TPost } from "../../types/posts";
import { toast, Toaster } from "sonner";

// ===================================================================================================================
// ===================================================================================================================

type CreatePostFormData = {
  title: string;
  content: string;
  uploadFiles: UploadFile[];
};

type UploadFile = {
  id: string;
  file: File;
  previewUrl: string;
};

type CreatePostValidationError = {
  title?: string;
  content?: string;
  uploadFiles?: string;
};

const initialState: CreatePostFormData = {
  title: "",
  content: "",
  uploadFiles: [],
};

const initialError: CreatePostValidationError = {
  title: undefined,
  content: undefined,
  uploadFiles: undefined,
};

// ===================================================================================================================
// ===================================================================================================================

export default function () {
  const [formData, setFormData] = useState<CreatePostFormData>(initialState);

  const [errors, setErrors] = useState<CreatePostValidationError>(initialError);

  const navigate = useNavigate();

  const fileInputRef = useRef<HTMLInputElement | null>(null);

  // -------------------------------------------------------------------------------------------------------------------

  const createPostMutation = useMutation({
    mutationFn: async (data: FormData): Promise<TPost> => {
      const response = await axios.post("/api/v1/posts", data, {
        withCredentials: true,
      });

      return response.data;
    },

    onSuccess: (data) => {
      toast.success("Post created. Redirecting...", {
        className: "!bg-green-100 !text-green-700 !text-base",
      });
      setTimeout(() => {
        navigate(`/posts/${data.id}`);
      }, 1000);
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

    // redundant - file picker intercepted if number of files is 5 in the upload files button
    if (formData.uploadFiles.length > 5) {
      setErrors((prev) => ({
        ...prev,
        uploadFiles: "Cannot upload more than 5 files.",
      }));
    } else {
      setErrors((prev) => ({ ...prev, uploadFiles: undefined }));
    }

    if (isReturn) return;

    const formDataSubmit = new FormData();

    formDataSubmit.append("title", formData.title);
    formDataSubmit.append("content", formData.content);

    formData.uploadFiles.forEach((uploadFile) => {
      formDataSubmit.append("files", uploadFile.file);
    });

    createPostMutation.mutate(formDataSubmit);
  };

  // -------------------------------------------------------------------------------------------------------------------

  const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };

  // -------------------------------------------------------------------------------------------------------------------

  const handleFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!e.target.files) return;

    const newFiles: UploadFile[] =
      Array.from(e.target.files).map((file) => ({
        id: crypto.randomUUID(),
        file,
        previewUrl: URL.createObjectURL(file),
      })) ?? [];

    setFormData((prev) => ({
      ...prev,
      uploadFiles: [...prev.uploadFiles, ...newFiles],
    }));

    e.target.value = "";
  };

  // -------------------------------------------------------------------------------------------------------------------

  return (
    <>
      <Toaster position="top-center" />
      <section className="flex flex-col gap-16">
        <h1 className="text-3xl font-medium text-gray-700 text-center">
          Create Post
        </h1>

        <form
          onSubmit={handleSubmit}
          className="flex flex-col gap-10 w-5/6 lg:w-3/4 mx-auto mb-10"
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
              htmlFor="content"
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

          <div className="flex flex-col gap-1">
            <input
              ref={fileInputRef}
              id="files"
              type="file"
              multiple
              onChange={handleFileChange}
              className="hidden"
            />
            <button
              type="button"
              onClick={() => {
                if (formData.uploadFiles.length == 5) {
                  toast.success("Maximum number of 5 files can be uploaded", {
                    className: "!bg-red-100 !text-red-700 !text-base",
                  });
                  return;
                }

                fileInputRef.current?.click();
              }}
              className="border border-gray-300 rounded-sm p-2 cursor-pointer font-medium text-gray-500 flex flex-col gap-1"
            >
              Upload files
            </button>
            {errors.uploadFiles && (
              <span className="text-sm font-semibold text-red-600">
                {errors.uploadFiles}
              </span>
            )}
          </div>

          {/* files preview */}
          {formData.uploadFiles.length > 0 && (
            <div className="flex gap-10">
              {formData.uploadFiles.map((uploadFile) => {
                return (
                  <div key={uploadFile.id} className="relative group w-40 h-40">
                    <img
                      src={uploadFile.previewUrl}
                      alt={`preview-${uploadFile.file.name}`}
                      className="w-full h-full object-cover"
                    />
                    <button
                      onClick={() => {
                        const newFiles = formData.uploadFiles.filter(
                          (fileToRemove) => uploadFile.id !== fileToRemove.id,
                        );

                        setFormData((prev) => ({
                          ...prev,
                          uploadFiles: newFiles,
                        }));
                      }}
                      className="absolute inset-0 bg-black/0 group-hover:bg-black/50 transition duration-300 text-white/0 group-hover:text-white cursor-pointer w-full"
                    >
                      <svg
                        xmlns="http://www.w3.org/2000/svg"
                        fill="none"
                        viewBox="0 0 24 24"
                        strokeWidth={1.5}
                        stroke="currentColor"
                        className="size-10 w-full text-center"
                      >
                        <path
                          strokeLinecap="round"
                          strokeLinejoin="round"
                          d="M6 18 18 6M6 6l12 12"
                        />
                      </svg>
                    </button>
                  </div>
                );
              })}
            </div>
          )}

          <button
            type="submit"
            className="bg-gray-900 text-white py-3 px-4 rounded-sm cursor-pointer font-semibold text-sm"
          >
            Create
          </button>
        </form>
      </section>
    </>
  );
}
