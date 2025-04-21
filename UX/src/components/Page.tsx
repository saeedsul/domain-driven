import React from 'react';
import Header from './Header';
import Body from './Content';
import Footer from './Footer';

const Page: React.FC = () => {
    return (
        <div className="container">
  

            <Header />
            <Body />
            <Footer /> 
        </div>
    );
};

export default Page;
