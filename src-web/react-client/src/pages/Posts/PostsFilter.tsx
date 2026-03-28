import { useState } from "react";
import type { PostsQuery } from "./Posts";

type Props = {
  filterFormData: PostsQuery;
  setFilterFormData: React.Dispatch<React.SetStateAction<PostsQuery>>;
};

export default function PostsFiler({
  filterFormData,
  setFilterFormData,
}: Props) {
  const [searchTerm, setSetTerm] = useState("");

  const handleFilterSubmit = (e: React.SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    setFilterFormData({ ...filterFormData, searchTerm: searchTerm });
  };

  return (
    <form
      onSubmit={handleFilterSubmit}
      className="flex flex-col lg:flex-row gap-2 text-sm w-full"
    >
      <input
        value={searchTerm}
        onChange={(e) => setSetTerm(e.target.value)}
        placeholder="Search posts..."
        className="border border-gray-300 rounded-sm p-2 flex-1"
      />

      <select
        className="border border-gray-300 rounded-sm p-2"
        value={filterFormData?.postOrderBy}
        onChange={(e) => {
          setFilterFormData({ ...filterFormData, postOrderBy: e.target.value });
        }}
      >
        <option value="createdAt">Created</option>
        <option value="updatedAt">Updated</option>
        <option value="title">Title</option>
      </select>

      <select
        className="border border-gray-300 rounded-sm p-2"
        value={filterFormData?.sortDirection}
        onChange={(e) =>
          setFilterFormData({
            ...filterFormData,
            sortDirection: e.target.value,
          })
        }
      >
        <option value="desc">Desc</option>
        <option value="asc">Asc</option>
      </select>

      <select
        className="border border-gray-300 rounded-sm p-2"
        onChange={(e) =>
          setFilterFormData({
            ...filterFormData,
            pageSize: parseInt(e.target.value),
          })
        }
      >
        <option value={10}>10/page</option>
        <option value={20}>20/page</option>
        <option value={30}>30/page</option>
      </select>

      <button
        type="submit"
        className="bg-gray-800 text-white py-2 px-4 rounded-sm cursor-pointer w-fit"
      >
        Filter
      </button>
    </form>
  );
}
