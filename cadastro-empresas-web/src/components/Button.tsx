'use client';

import { ButtonHTMLAttributes, FC } from 'react';

const Button: FC<ButtonHTMLAttributes<HTMLButtonElement>> = ({ children, ...props }) => {
  return (
    <button
      {...props}
      className="w-full bg-[#0057B7] text-white font-medium py-2 px-4 rounded-md transition duration-200 hover:bg-[#004aa0] disabled:opacity-50 disabled:cursor-not-allowed cursor-pointer"
    >
      {children}
    </button>
  );
};

export default Button;
